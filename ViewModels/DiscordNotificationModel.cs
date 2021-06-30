using System.Text.Json.Serialization;

namespace OrdersApi.ViewModels
{
    public class DiscordNotificationModel
    {
        public DiscordNotificationModel(
            string webHookUrl,
            string content,
            string username = "balta.io",
            string avatarUrl = "https://baltaio.blob.core.windows.net/static/images/logos/icone-novo-dark-flat.png")
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            WebHookUrl = webHookUrl;
        }

        [JsonPropertyName("content")] public string Content { get; set; }

        [JsonPropertyName("username")] public string Username { get; set; }

        [JsonPropertyName("avatar_url")] public string AvatarUrl { get; set; }

        [JsonIgnore] public string WebHookUrl { get; set; }
    }
}
