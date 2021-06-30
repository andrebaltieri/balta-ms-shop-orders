using System;

namespace OrdersApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Product { get; set; }
        public decimal Price { get; set; }
    }
}