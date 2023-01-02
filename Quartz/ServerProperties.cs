using System.Reflection;
using System.Text;

namespace Quartz
{
    internal class ServerProperties
    {
        public bool EnableJmxMonitoring { get; set; } = false;
        public int RconPort { get; set; } = 25575;
        public string LevelSeed { get; set; } = "";
        public Gamemode Gamemode { get; set; } = Gamemode.Survival;
        public bool EnableCommandBlock { get; set; } = false;
        public bool EnableQuery { get; set; } = false;
        public string GeneratorSettings { get; set; } = "{}";
        public bool EnforceSecureProfile { get; set; } = true;
        public string LevelName { get; set; } = "world";
        public string MOTD { get; set; } = "A Minecraft Server";
        public int QueryPort { get; set; } = 25565;
        public bool PVP { get; set; } = true;
        public bool GenerateStructures { get; set; } = true;
        public int MaxChainedNeighborUpdates { get; set; } = 1000000;
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public int NetworkCompressionThreshold { get; set; } = 256;
        public int MaxTickTime { get; set; } = 60000;
        public bool RequireResourcePack { get; set; } = false;
        public bool UseNativeTransport { get; set; } = true;
        public int MaxPlayers { get; set; } = 20;
        public bool OnlineMode { get; set; } = true;
        public bool EnableStatus { get; set; } = true;
        public bool AllowFlight { get; set; } = false;
        public string InitialDisabledPack { get; set; } = "";
        public bool BroadcastRconToOps { get; set; } = true;
        public int ViewDistance { get; set; } = 10;
        public string ServerIp { get; set; } = "";
        public string ResourcePackPrompt { get; set; } = "";
        public bool AllowNether { get; set; } = true;
        public int ServerPort { get; set; } = 25565;
        public bool EnableRcon { get; set; } = false;
        public bool SyncChunkWrites { get; set; } = true;
        public int OpPermissionLevel { get; set; } = 4;
        public bool PreventProxyConnections { get; set; } = false;
        public bool HideOnlinePlayers { get; set; } = false;
        public string ResourcePack { get; set; } = "";
        public int EntityBroadcastRangePercentage { get; set; } = 100;
        public int SimulationDistance { get; set; } = 10;
        public string RconPassword { get; set; } = "";
        public int PlayerIdleTimeout { get; set; } = 0;
        public bool ForceGamemode { get; set; } = false;
        public int RateLimit { get; set; } = 0;
        public bool Debug { get; set; } = false;
        public bool Hardcore { get; set; } = false;
        public bool WhiteList { get; set; } = false;
        public bool BroadcastConsoleToOps { get; set; } = true;
        public bool SpawnNpcs { get; set; } = true;
        public bool PreviewsChat { get; set; } = false;
        public bool SpawnAnimals { get; set; } = true;
        public int FunctionPermissionLevel { get; set; } = 2;
        public string InitialEnabledPacks { get; set; } = "vanilla";
        public string LevelType { get; set; } = "minecraft\\:normal";
        public string TextFilteringConfig { get; set; } = "";
        public bool SpawnMonsters { get; set; } = true;
        public bool EnforceWhitelist { get; set; } = true;
        public int SpawnProtection { get; set; } = 16;
        public string ResourcePackSha1 { get; set; } = "";
        public int MaxWorldSize { get; set; } = 29999984;

        public string GetServerProperties()
        {
            var properties = this.GetType().GetProperties();
            var sb = new StringBuilder();

            foreach (var property in properties)
            {

                //foreach (var letter in property.Name)
                //{
                //    if (property.Name == "MOTD" || property.Name == "PVP")
                //    {
                //        name = property.Name == "MOTD" ? "motd" : "pvp";
                //        break;
                //    }

                //    var value = string.Empty;
                    
                //    // If it's upper, adds a '-' before.
                //    if (char.IsUpper(letter) && index != 0)
                //    {
                //        if (property.Name.Contains("query") || property.Name.Contains("rcon"))
                //            value += '.';
                //        else
                //            value += '-';
                //    }

                //    value += letter.ToString().ToLower();
                //    name += letter;
                //    index++;
                //}

                sb.AppendLine(GetName(property.Name) + "=" + (property.GetValue(this) ?? "").ToString().ToLower());
            }

            return sb.ToString();
        }

        public static ServerProperties Deserialize(string serverProperties)
        {
            var server = new ServerProperties();
            var dictionary = GetDictionaryProperties(serverProperties);

            // Basic Settings
            server.LevelName = dictionary["level-name"];
            server.LevelSeed = dictionary["level-seed"];
            server.MaxPlayers = int.Parse(dictionary["max-players"]);
            server.Gamemode = ConvertGamemode(dictionary["gamemode"]);
            server.Difficulty = ConvertDifficulty(dictionary["difficulty"]);
            server.GenerateStructures = ConvertToBool(dictionary["generate-structures"]);
            server.OnlineMode = ConvertToBool(dictionary["online-mode"]);
            server.AllowNether = ConvertToBool(dictionary["allow-nether"]);
            server.Hardcore = ConvertToBool(dictionary["hardcore"]);
            server.SpawnNpcs = ConvertToBool(dictionary["spawn-npcs"]);
            server.SpawnAnimals = ConvertToBool(dictionary["spawn-animals"]);
            server.SpawnMonsters = ConvertToBool(dictionary["spawn-monsters"]);
            server.PVP = ConvertToBool(dictionary["pvp"]);
            server.MOTD = dictionary["motd"];

            // Server Settings
            server.ServerIp = dictionary["server-ip"];
            server.ServerPort = int.Parse(dictionary["server-port"]);
            server.WhiteList = ConvertToBool(dictionary["white-list"]);
            server.EnforceWhitelist = ConvertToBool(dictionary["enforce-whitelist"]);
            server.ResourcePack = dictionary["resource-pack"];
            server.ResourcePackPrompt = dictionary["resource-pack-prompt"];
            server.ResourcePackSha1 = dictionary["resource-pack-sha1"];
            server.RequireResourcePack = ConvertToBool(dictionary["require-resource-pack"]);

            // Op Settings
            server.BroadcastRconToOps = ConvertToBool(dictionary["broadcast-rcon-to-ops"]); 
            server.BroadcastConsoleToOps = ConvertToBool(dictionary["broadcast-console-to-ops"]);
            server.OpPermissionLevel = int.Parse(dictionary["op-permission-level"]);

            // Network Settings
            server.NetworkCompressionThreshold = int.Parse(dictionary["network-compression-threshold"]);
            server.RateLimit = int.Parse(dictionary["rate-limit"]);
            server.PreventProxyConnections = ConvertToBool(dictionary["prevent-proxy-connections"]);
            server.MaxTickTime = int.Parse(dictionary["max-tick-time"]);
            server.PreviewsChat = ConvertToBool(dictionary["previews-chat"]);
            server.EnableQuery = ConvertToBool(dictionary["enable-query"]);
            server.QueryPort = int.Parse(dictionary["query.port"]);

            // Performance Settings
            server.ViewDistance = int.Parse(dictionary["view-distance"]);
            server.SimulationDistance = int.Parse(dictionary["simulation-distance"]);
            server.EntityBroadcastRangePercentage = int.Parse(dictionary["entity-broadcast-range-percentage"]);
            server.MaxChainedNeighborUpdates = int.Parse(dictionary["max-chained-neighbor-updates"]);
            server.UseNativeTransport = ConvertToBool(dictionary["use-native-transport"]);

            // Misc Settings
            server.MaxWorldSize = int.Parse(dictionary["max-world-size"]);
            server.EnableStatus = ConvertToBool(dictionary["enable-status"]);
            server.AllowFlight = ConvertToBool(dictionary["allow-flight"]);
            server.HideOnlinePlayers = ConvertToBool(dictionary["hide-online-players"]);
            server.PlayerIdleTimeout = int.Parse(dictionary["player-idle-timeout"]);
            server.GeneratorSettings = dictionary["generator-settings"];
            server.SpawnProtection = int.Parse(dictionary["spawn-protection"]);
            server.TextFilteringConfig = dictionary["text-filtering-config"];
            server.EnableRcon = ConvertToBool(dictionary["enable-rcon"]);
            server.RconPassword = dictionary["rcon.password"];
            server.RconPort = int.Parse(dictionary["rcon.port"]);
            server.LevelType = dictionary["level-type"];
            server.Debug = ConvertToBool(dictionary["debug"]);
            server.SyncChunkWrites = ConvertToBool(dictionary["sync-chunk-writes"]);
            server.EnableJmxMonitoring = ConvertToBool(dictionary["enable-jmx-monitoring"]);
            server.EnableCommandBlock = ConvertToBool(dictionary["enable-command-block"]);
            server.ForceGamemode = ConvertToBool(dictionary["force-gamemode"]);
            server.FunctionPermissionLevel = int.Parse(dictionary["function-permission-level"]);

            return server;
        }

        private static IReadOnlyDictionary<string, string> GetDictionaryProperties(string serverProperties)
        {
            var dictionary = new Dictionary<string, string>();
            var sr = new StringReader(serverProperties);

            string? text;
            while ((text = sr.ReadLine()) != null)
            {
                if (text.StartsWith('#'))
                {
                    continue;
                }
                else
                {
                    var property = text[0..text.IndexOf('=')];
                    var value = text[(text.IndexOf('=') + 1)..text.Length];
                    dictionary.Add(GetName(property), value.ToLower());
                }
            }

            return dictionary;
        }

        private static string GetName(string name)
        {
            if (IsAllUpper(name) && !(name.Contains('-') || name.Contains('.')))
                return name.ToLower();
            
            var sb = new StringBuilder();

            bool initial = true;
            foreach (var ch in name)
            {
                if (initial)
                {
                    initial = false;
                }
                else if (char.IsUpper(ch) && !initial)
                {
                    if (name == "QueryPort" || name == "RconPort" || name == "RconPassword")
                    {
                        sb.Append('.');
                    }
                    else
                    {
                        sb.Append('-');
                    }
                }

                sb.Append(ch.ToString().ToLower());
            }

            return sb.ToString();
        }

        private static bool IsAllUpper(string text)
        {
            foreach (var ch in text)
            {
                if (char.IsLower(ch))
                    return false;
            }

            return true;
        }

        public static Gamemode ConvertGamemode(string text)
        {
            return text switch
            {
                "survival" => Gamemode.Survival,
                "creative" => Gamemode.Creative,
                "adventure" => Gamemode.Adventure,
                "spectator" => Gamemode.Spectator,
                _ => throw new NotImplementedException()
            };
        }

        public static Difficulty ConvertDifficulty(string text)
        {
            return text switch
            {
                "peaceful" => Difficulty.Peaceful,
                "easy" => Difficulty.Easy,
                "normal" => Difficulty.Normal,
                "hard" => Difficulty.Hard,
                _ => throw new NotImplementedException()
            };
        }

        private static bool ConvertToBool(string text)
        {
            return text.ToLower() == "true";
        }
    }

    public enum Gamemode
    {
        Survival,
        Creative,
        Spectator,
        Adventure
    }

    public enum Difficulty
    {
        Peaceful,
        Easy,
        Normal,
        Hard 
    }
}
