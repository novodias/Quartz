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

        [JsonIgnore] private string? _url;

        [JsonProperty("enabled")] public bool Enabled { get; set; }
        
        [JsonProperty("url")]
        public string? Url 
        { 
            get
            {
                return _url;
            }
        }

        static MinecraftWebhook()
        {
            Name = "webhook.json";
        }

        public MinecraftWebhook()
        {
            _url = "";
            Enabled = false;
            _webhookClient = null;
        }

        public MinecraftWebhook(string url, bool enabled)
        {
            _url = url;
            Enabled = enabled;
            _webhookClient = new(_url);
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
            if (!url.StartsWith("https://") || !url.Contains("discord.com/api/webhooks"))
            {
                return false;
            }

            _url = url;

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

        public static MinecraftWebhook LoadOrCreate()
        {
            var result = Load<MinecraftWebhook>(Name);

            if (result != null)
            {
                return result;
            }

            var webhook = new MinecraftWebhook();
            webhook.Save();
            return webhook;

            //var path = Path.Combine(AppContext.BaseDirectory, _name);

            //if (!File.Exists(path)) 
            //{
            //    return Create();
            //}

            //using (var sr = new StreamReader(path))
            //{
            //    var content = sr.ReadToEnd();
            //    webhook = JsonConvert.DeserializeObject<MinecraftWebhook>(content);
            //}

            //if (webhook == null)
            //{
            //    return Create();
            //}

            //return webhook;
        }

        //public void Save()
        //{
        //    var path = Path.Combine(AppContext.BaseDirectory, _name);

        //    using (var sw = new StreamWriter(path, false))
        //    {
        //        var content = JsonConvert.SerializeObject(this);
        //        sw.WriteLine(content);
        //    }
        //}
    }
}
