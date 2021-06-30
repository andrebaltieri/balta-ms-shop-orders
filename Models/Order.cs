using System;
using System.Collections.Generic;

namespace OrdersApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}