using System.Collections.Generic;
using OrdersApi.Models;

namespace OrdersApi.ViewModels
{
    public class CreateOrderViewModel
    {
        public string Customer { get; set; }
        public List<Product> Products { get; set; }
    }
}