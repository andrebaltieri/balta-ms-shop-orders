using System;

namespace OrdersApi.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}