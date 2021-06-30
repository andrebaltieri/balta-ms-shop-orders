using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Enums;
using OrdersApi.Models;
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
            int id,
            [FromServices] StoreDataContext context)
        {
            var orders = await context.Orders.AsNoTracking().Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
            return Ok(orders);
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateOrderViewModel model,
            [FromServices] StoreDataContext context)
        {
            var order = new Order
            {
                Customer = model.Customer,
                Date = DateTime.Now,
                Number = Guid.NewGuid().ToString().Substring(0, 8),
                Status = EOrderStatus.Paid,
                Id = 0,
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

            return Ok(order);
        }
    }
}