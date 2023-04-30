using Newtonsoft.Json;

namespace Quartz.Forms
{
    internal class Presets
    {
        private static readonly string _directory = AppContext.BaseDirectory;
        private static readonly string _name = "presets.json";
        
        [JsonProperty("servers")]
        private readonly IList<Server> Servers;

        public Presets()
        {
            Servers = new List<Server>(); 
        }

        public static Presets? Deserialize()
        {
            Presets? presets;
            
            var path = Path.Combine(_directory, _name);
            
            if (!File.Exists(path))
            {
                return null;
            }

            using (var sw = new StreamReader(path))
            {
                var content = sw.ReadToEnd();
                presets = JsonConvert.DeserializeObject<Presets>(content);
            }

            if (presets == null)
            {
                return null;
            }

            return presets;
        }

        public Server this[int index] => Servers[index];

        [JsonIgnore] public int Count => Servers.Count;

        public void Add(Server server) => Servers.Add(server);

        public void Remove(Server server) => Servers.Remove(server);

        public void Save()
        {
            var path = Path.Combine(_directory, _name);
            
            using (var sw = new StreamWriter(path, false))
            {
                var content = JsonConvert.SerializeObject(this);

                sw.WriteLine(content);
            }
        }
    }

    public struct Server
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("path")]
        public string Path;

        [JsonProperty("memory")]
        public string MemoryAllocated;

        [JsonProperty("java")]
        public int JavaIndex;
    }
}
