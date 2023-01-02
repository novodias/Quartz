using Discord.Webhook;
using Newtonsoft.Json;

namespace Quartz
{
    internal class MinecraftWebhook
    {
        private static readonly string _name = "webhook.json";

        [JsonIgnore] 
        private DiscordWebhookClient? _webhookClient;

        [JsonIgnore]
        private string? _url;

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        
        [JsonProperty("url")]
        public string? Url 
        { 
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                if (!string.IsNullOrWhiteSpace(value)) 
                { 
                    _webhookClient = new DiscordWebhookClient(value);
                }
            }
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
            _webhookClient = new(_url);
            
            Enabled = enabled;
        }

        public async Task PostAsync(string player, string message)
        {
            if (!Enabled || _webhookClient is null) 
            {
                return;
            }
            
            await _webhookClient.SendMessageAsync(text:message, username:player);
        }

        public static MinecraftWebhook LoadOrCreate()
        {
            MinecraftWebhook? webhook;
            
            static MinecraftWebhook Create()
            {
                var webhook = new MinecraftWebhook();
                webhook.Save();
                return webhook;
            }

            var path = Path.Combine(AppContext.BaseDirectory, _name);

            if (!File.Exists(path)) 
            {
                return Create();
            }

            using (var sr = new StreamReader(path))
            {
                var content = sr.ReadToEnd();
                webhook = JsonConvert.DeserializeObject<MinecraftWebhook>(content);
            }

            if (webhook == null)
            {
                return Create();
            }

            return webhook;
        }

        public void Save()
        {
            var path = Path.Combine(AppContext.BaseDirectory, _name);

            using (var sw = new StreamWriter(path, false))
            {
                var content = JsonConvert.SerializeObject(this);
                sw.WriteLine(content);
            }
        }
    }
}
