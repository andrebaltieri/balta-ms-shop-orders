using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OrdersApi.ViewModels;

namespace OrdersApi.Services
{
    public class NotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration) => _configuration = configuration;

        public async Task NotifyAsync(string customer, string message)
        {
            var notification = new DiscordNotificationModel(
                _configuration["DiscordWebHookUrl"],
                message,
                customer);


            var httpClient = new HttpClient();
            var data = JsonSerializer.Serialize(notification, new JsonSerializerOptions
            {
                IgnoreNullValues = true
            });
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(notification.WebHookUrl, content);
        }
    }
}
