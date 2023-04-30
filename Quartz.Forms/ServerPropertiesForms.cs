namespace Quartz.Forms
{
    public partial class ServerPropertiesForms : Form
    {
        private FileInfo _serverPropertiesFile;
        private ServerProperties ServerProperties;

        public ServerPropertiesForms(DirectoryInfo directory)
        {
            InitializeComponent();

            var files = directory.GetFiles();

            var serverPropertiesFile = files.FirstOrDefault(f => f.Name.ToLower().Contains("server.properties"));
            if (serverPropertiesFile is null)
            {
                ServerProperties = new();
                _serverPropertiesFile = new(Path.Join(directory.FullName, "server.properties"));
                File.WriteAllText(_serverPropertiesFile.FullName, ServerProperties.GetServerProperties());
            }
            else
            {
                _serverPropertiesFile = serverPropertiesFile;
                var config = File.ReadAllText(_serverPropertiesFile.FullName);
                ServerProperties = ServerProperties.Deserialize(config);
            }

            SetBasicSettings();
            SetServerSettings();
            SetOpSettings();
            SetNetworkSettings();
            SetPerformanceSettings();
            SetMiscSettings();
        }

        private void SetBasicSettings()
        {
            LevelNameTextBox.Text = ServerProperties.LevelName;
            LevelSeedTextBox.Text = ServerProperties.LevelSeed;
            MaxPlayersTextBox.Text = ServerProperties.MaxPlayers.ToString();
            
            GamemodeComboBox.Items.Add(Gamemode.Survival);
            GamemodeComboBox.Items.Add(Gamemode.Creative);
            GamemodeComboBox.Items.Add(Gamemode.Adventure);
            GamemodeComboBox.Items.Add(Gamemode.Spectator);
            GamemodeComboBox.SelectedItem = ServerProperties.Gamemode;

            DifficultyComboBox.Items.Add(Difficulty.Peaceful);
            DifficultyComboBox.Items.Add(Difficulty.Easy);
            DifficultyComboBox.Items.Add(Difficulty.Normal);
            DifficultyComboBox.Items.Add(Difficulty.Hard);
            DifficultyComboBox.SelectedItem = ServerProperties.Difficulty;

            GenerateStructuresCheckBox.Checked = ServerProperties.GenerateStructures;
            OnlineModeCheckBox.Checked = ServerProperties.OnlineMode;
            AllowNetherCheckBox.Checked = ServerProperties.AllowNether;
            HardcoreCheckBox.Checked = ServerProperties.Hardcore;
            SpawnNPCSCheckBox.Checked = ServerProperties.SpawnNpcs;
            SpawnAnimalsCheckBox.Checked = ServerProperties.SpawnAnimals;
            SpawnMonstersCheckBox.Checked = ServerProperties.SpawnMonsters;
            PVPCheckBox.Checked = ServerProperties.PVP;
            MOTDTextBox.Text = ServerProperties.MOTD;
        }

        private void SetServerSettings()
        {
            ServerIPTextBox.Text = ServerProperties.ServerIp;
            ServerPortTextBox.Text = ServerProperties.ServerPort.ToString();
            WhitelistCheckBox.Checked = ServerProperties.WhiteList; 
            EnforceWhitelistCheckBox.Checked = ServerProperties.EnforceWhitelist;
            ResourcePackTextBox.Text = ServerProperties.ResourcePack;
            ResourcePackPromptTextBox.Text = ServerProperties.ResourcePackPrompt;
            ResourcePackSHA1TextBox.Text = ServerProperties.ResourcePackSha1;
            RequireResourcePackCheckBox.Checked = ServerProperties.RequireResourcePack;
        }

        private void SetOpSettings()
        {
            BroadcastRconToOpsCheckBox.Checked = ServerProperties.BroadcastRconToOps;
            BroadcastConsoleToOpsCheckBox.Checked = ServerProperties.BroadcastConsoleToOps;
            OpPermissionLevelComboBox.Items.Add(0);
            OpPermissionLevelComboBox.Items.Add(1);
            OpPermissionLevelComboBox.Items.Add(2);
            OpPermissionLevelComboBox.Items.Add(3);
            OpPermissionLevelComboBox.Items.Add(4);
            OpPermissionLevelComboBox.SelectedItem = ServerProperties.OpPermissionLevel;
        }

        private void SetNetworkSettings()
        {
            NetworkCompressionThresholdTextBox.Text = ServerProperties.NetworkCompressionThreshold.ToString();
            RateLimitTextBox.Text = ServerProperties.RateLimit.ToString();
            PreventProxyConnectionsCheckBox.Checked = ServerProperties.PreventProxyConnections;
            MaxTickTimeTextBox.Text = ServerProperties.MaxTickTime.ToString();
            PreviewsChatCheckBox.Checked = ServerProperties.PreviewsChat;
            EnableQueryCheckBox.Checked = ServerProperties.EnableQuery;
            QueryPortTextBox.Text = ServerProperties.QueryPort.ToString();
        }

        private void SetPerformanceSettings()
        {
            ViewDistanceTextBox.Text = ServerProperties.ViewDistance.ToString();
            SimulationDistanceTextBox.Text = ServerProperties.SimulationDistance.ToString();
            EntityBroadcastRangePercentageTextBox.Text = ServerProperties.EntityBroadcastRangePercentage.ToString();
            MaxChainedNeighborUpdatesTextBox.Text = ServerProperties.MaxChainedNeighborUpdates.ToString();
            UseNativeTransportCheckBox.Checked = ServerProperties.UseNativeTransport;
        }

        private void SetMiscSettings()
        {
            MaxWorldSizeTextBox.Text = ServerProperties.MaxWorldSize.ToString();
            EnableStatusCheckBox.Checked = ServerProperties.EnableStatus;
            AllowFlightCheckBox.Checked = ServerProperties.AllowFlight;
            HideOnlinePlayersCheckBox.Checked = ServerProperties.HideOnlinePlayers;
            PlayerIdleTimeoutTextBox.Text = ServerProperties.PlayerIdleTimeout.ToString();
            GeneratorSettingsTextBox.Text = ServerProperties.GeneratorSettings;
            SpawnProtectionTextBox.Text = ServerProperties.SpawnProtection.ToString();
            TextFilteringConfigTextBox.Text = ServerProperties.TextFilteringConfig;
            EnableRconCheckBox.Checked = ServerProperties.EnableRcon;
            RconPasswordTextBox.Text = ServerProperties.RconPassword;
            RconPortTextBox.Text = ServerProperties.RconPort.ToString();
            LevelTypeTextBox.Text = ServerProperties.LevelType;
            DebugCheckBox.Checked = ServerProperties.Debug;
            SyncChunkWritesCheckBox.Checked = ServerProperties.SyncChunkWrites;
            EnableJMXMonitoringCheckBox.Checked = ServerProperties.EnableJmxMonitoring;
            EnableCommandBlockCheckBox.Checked = ServerProperties.EnableCommandBlock;
            ForceGamemodeCheckBox.Checked = ServerProperties.ForceGamemode;
            FunctionPermissionLevelComboBox.Items.Add(1);
            FunctionPermissionLevelComboBox.Items.Add(2);
            FunctionPermissionLevelComboBox.Items.Add(3);
            FunctionPermissionLevelComboBox.Items.Add(4);
            FunctionPermissionLevelComboBox.SelectedItem = ServerProperties.FunctionPermissionLevel;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Basic Settings
            ServerProperties.LevelName = LevelNameTextBox.Text;
            ServerProperties.LevelSeed = LevelSeedTextBox.Text;
            ServerProperties.MaxPlayers = int.Parse(MaxPlayersTextBox.Text);
            ServerProperties.Gamemode = (Gamemode)GamemodeComboBox.SelectedItem;
            ServerProperties.Difficulty = (Difficulty)DifficultyComboBox.SelectedItem;
            ServerProperties.GenerateStructures = GenerateStructuresCheckBox.Checked;
            ServerProperties.OnlineMode = OnlineModeCheckBox.Checked;
            ServerProperties.AllowNether = AllowNetherCheckBox.Checked;
            ServerProperties.Hardcore = HardcoreCheckBox.Checked;
            ServerProperties.SpawnNpcs = SpawnNPCSCheckBox.Checked;
            ServerProperties.SpawnAnimals = SpawnAnimalsCheckBox.Checked;
            ServerProperties.SpawnMonsters = SpawnMonstersCheckBox.Checked;
            ServerProperties.PVP = PVPCheckBox.Checked;
            ServerProperties.MOTD = MOTDTextBox.Text;

            // Server Settings
            ServerProperties.ServerIp = ServerIPTextBox.Text;
            ServerProperties.ServerPort = int.Parse(ServerPortTextBox.Text);
            ServerProperties.WhiteList = WhitelistCheckBox.Checked;
            ServerProperties.EnforceWhitelist = EnforceWhitelistCheckBox.Checked;
            ServerProperties.ResourcePack = ResourcePackTextBox.Text;
            ServerProperties.ResourcePackPrompt = ResourcePackPromptTextBox.Text;
            ServerProperties.ResourcePackSha1 = ResourcePackSHA1TextBox.Text;
            ServerProperties.RequireResourcePack = RequireResourcePackCheckBox.Checked;

            // Op Settings
            ServerProperties.BroadcastRconToOps = BroadcastRconToOpsCheckBox.Checked;
            ServerProperties.BroadcastConsoleToOps = BroadcastConsoleToOpsCheckBox.Checked;
            ServerProperties.OpPermissionLevel = (int)OpPermissionLevelComboBox.SelectedItem;

            // Network Settings
            ServerProperties.NetworkCompressionThreshold = int.Parse(NetworkCompressionThresholdTextBox.Text);
            ServerProperties.RateLimit = int.Parse(RateLimitTextBox.Text);
            ServerProperties.PreventProxyConnections = PreventProxyConnectionsCheckBox.Checked;
            ServerProperties.MaxTickTime = int.Parse(MaxTickTimeTextBox.Text);
            ServerProperties.PreviewsChat = PreviewsChatCheckBox.Checked;
            ServerProperties.EnableQuery = EnableQueryCheckBox.Checked;
            ServerProperties.QueryPort = int.Parse(QueryPortTextBox.Text);

            // Performance Settings
            ServerProperties.ViewDistance = int.Parse(ViewDistanceTextBox.Text);
            ServerProperties.SimulationDistance = int.Parse(SimulationDistanceTextBox.Text);
            ServerProperties.EntityBroadcastRangePercentage = int.Parse(EntityBroadcastRangePercentageTextBox.Text);
            ServerProperties.MaxChainedNeighborUpdates = int.Parse(MaxChainedNeighborUpdatesTextBox.Text);
            ServerProperties.UseNativeTransport = UseNativeTransportCheckBox.Checked;

            // Misc Settings
            ServerProperties.MaxWorldSize = int.Parse(MaxWorldSizeTextBox.Text);
            ServerProperties.EnableStatus = EnableStatusCheckBox.Checked;
            ServerProperties.AllowFlight = AllowFlightCheckBox.Checked;
            ServerProperties.HideOnlinePlayers = HideOnlinePlayersCheckBox.Checked;
            ServerProperties.PlayerIdleTimeout = int.Parse(PlayerIdleTimeoutTextBox.Text);
            ServerProperties.GeneratorSettings = GeneratorSettingsTextBox.Text;
            ServerProperties.SpawnProtection = int.Parse(SpawnProtectionTextBox.Text);
            ServerProperties.TextFilteringConfig = TextFilteringConfigTextBox.Text;
            ServerProperties.EnableRcon = EnableRconCheckBox.Checked;
            ServerProperties.RconPassword = RconPasswordTextBox.Text;
            ServerProperties.RconPort = int.Parse(RconPortTextBox.Text);
            ServerProperties.LevelType = LevelTypeTextBox.Text;
            ServerProperties.Debug = DebugCheckBox.Checked;
            ServerProperties.SyncChunkWrites = SyncChunkWritesCheckBox.Checked;
            ServerProperties.EnableJmxMonitoring = EnableJMXMonitoringCheckBox.Checked;
            ServerProperties.EnableCommandBlock = EnableCommandBlockCheckBox.Checked;
            ServerProperties.ForceGamemode = ForceGamemodeCheckBox.Checked;
            ServerProperties.FunctionPermissionLevel = (int)FunctionPermissionLevelComboBox.SelectedItem;

            var serverProperties = ServerProperties.GetServerProperties();
            File.WriteAllText(_serverPropertiesFile.FullName, serverProperties);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ServerProperties = new();
            SetBasicSettings();
            SetServerSettings();
            SetOpSettings();
            SetNetworkSettings();
            SetPerformanceSettings();
            SetMiscSettings();
        }
    }
}
