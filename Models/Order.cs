using System;
using System.Collections.Generic;
using System.Linq;
using OrdersApi.Enums;

namespace OrdersApi.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public EOrderStatus Status { get; set; }
        public decimal Total => Items.Sum(x => x.Price);
    }
}