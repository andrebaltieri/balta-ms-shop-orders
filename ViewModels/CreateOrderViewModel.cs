namespace OrdersApi.ViewModels
{
    public class CreateOrderViewModel
    {
        public string Customer { get; set; }
        public int[] Products { get; set; }
    }
}