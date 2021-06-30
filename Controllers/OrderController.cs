using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrdersApi.Data;
using OrdersApi.Enums;
using OrdersApi.Models;
using OrdersApi.Services;
using OrdersApi.ViewModels;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {
        [HttpGet("customers/{customer}")]
        public async Task<IActionResult> GetAsync(
            string customer,
            [FromServices] StoreDataContext context)
        {
            var orders = await context.Orders.AsNoTracking().Where(x => x.Customer == customer).ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByAsync(
            string number,
            [FromServices] StoreDataContext context)
        {
            var orders = await context.Orders.AsNoTracking().Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Number == number);
            return Ok(orders);
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateOrderViewModel model,
            [FromServices] StoreDataContext context,
            [FromServices] NotificationService notificationService,
            [FromServices] MessageBusService messageBus,
            [FromServices] IConfiguration config)
        {
            try
            {
                var order = new Order
                {
                    Customer = model.Customer,
                    Date = DateTime.Now,
                    Number = Guid.NewGuid().ToString().Substring(0, 8),
                    Status = EOrderStatus.Paid,
                };

                foreach (var item in model.Products)
                {
                    order.Items.Add(new OrderItem
                    {
                        Price = item.Price,
                        Product = item
                    });
                }

                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();

                await messageBus.SendAsync(order);
                await notificationService.NotifyAsync(order.Customer, $"Olá {order.Customer}, seu pedido {order.Number} foi realizado com sucesso!");

                return Ok(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutAsync(
            [FromQuery] Guid id,
            [FromBody] UpdateOrderStatusViewModel model,
            [FromServices] StoreDataContext context,
            [FromServices] NotificationService notificationService,
            [FromServices] MessageBusService messageBus)
        {
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            order.Status = model.Status;

            context.Orders.Update(order);
            await context.SaveChangesAsync();

            var notification = new StringBuilder();
            notification.Append($"Olá {order.Customer}, ");
            switch (order.Status)
            {
                case EOrderStatus.Created:
                    notification.Append($"seu pedido {order.Number} foi criado!");
                    break;
                case EOrderStatus.WaitingPayment:
                    notification.Append($"seu pedido {order.Number} está aguardando pagamento");
                    break;
                case EOrderStatus.Paid:
                    notification.Append($"seu pedido {order.Number} foi pago!");
                    break;
                case EOrderStatus.Shipping:
                    {
                        await messageBus.SendAsync(order, "shipping");
                        notification.Append($"seu pedido {order.Number} foi enviado!");
                        break;
                    }
                case EOrderStatus.Delivered:
                    notification.Append($"seu pedido {order.Number} foi entregue!");
                    break;
                case EOrderStatus.Canceled:
                    notification.Append($"seu pedido {order.Number} foi cancelado!");
                    break;
            }

            await notificationService.NotifyAsync(order.Customer, notification.ToString());

            return Ok(order);
        }
    }
}