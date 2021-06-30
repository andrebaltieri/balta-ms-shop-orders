namespace OrdersApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}