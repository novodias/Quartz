using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using Quartz.Avalonia.Files;
using System;
using System.Threading.Tasks;

namespace QuartzAvalonia.Files
{
    public class MinecraftWebhook : FileConfiguration
    {
        [JsonIgnore] private DiscordWebhookClient? _webhookClient;

        [JsonProperty("enabled")] public bool Enabled { get; set; }
        
        [JsonProperty("url")] public string? Url { get; private set; }

        public MinecraftWebhook()
        {
            Name = "webhook.json";
            Url = "";
            Enabled = false;
            _webhookClient = null;
        }

        public MinecraftWebhook(string url, bool enabled)
        {
            Name = "webhook.json";
            Url = url;
            Enabled = enabled;
            _webhookClient = new(Url);
        }

        public async Task PostAsync(string player, string message)
        {
            if (!Enabled || _webhookClient is null) 
            {
                return;
            }
            
            await _webhookClient.SendMessageAsync(text:message, username:player);
        }

        public bool TrySetWebhookUrl(string url) 
        {
            if (string.IsNullOrWhiteSpace(url) || 
                !url.StartsWith("https://") || 
                !url.Contains("discord.com/api/webhooks") ||
                url == Url)
            {
                return false;
            }

            Url = url;

            return true;
        }

        public void Init()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new NullReferenceException(nameof(Url));
            }

            _webhookClient = new DiscordWebhookClient(Url);
        }

        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                Enabled = false;
            }
            
            base.Save();
        }

        public static MinecraftWebhook LoadOrCreate()
        {
            var result = Load<MinecraftWebhook>("webhook.json");

            if (result != null)
            {
                return result;
            }

            result = new MinecraftWebhook();
            result.Save();
            return result;
        }
    }
}
