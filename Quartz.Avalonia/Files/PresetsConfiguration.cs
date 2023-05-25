using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Quartz.Avalonia.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.Files
{
    public class PresetsConfiguration : FileConfiguration
    {
        [JsonProperty("servers")] private readonly IList<Server> _servers;

        [JsonIgnore] public IReadOnlyList<Server> Servers => _servers.AsReadOnly();

        public PresetsConfiguration()
        {
            Name = "presets.json";
            _servers = new List<Server>();
        }

        public static PresetsConfiguration LoadOrCreate()
        {
            var result = Load<PresetsConfiguration>("presets.json");

            if (result != null) 
            {
                return result;
            }

            result = new PresetsConfiguration();
            result.Save();
            return result;
        }

        public Server this[int index] => Servers[index];

        [JsonIgnore] public int Count => Servers.Count;

        public void Add(Server server) => _servers.Add(server);

        public void Remove(Server server) => _servers.Remove(server);
    }

    public struct Server
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("memory")]
        public string Memory { get; set; }

        [JsonProperty("java")]
        public int JavaIndex { get; set; }
    }
}
