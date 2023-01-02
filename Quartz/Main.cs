using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace Quartz
{
    public partial class Main : Form
    {
        // Process related
        private Process? _process = null;
        private TextBuilder outputBuilder = new TextBuilder();

        private MinecraftWebhook _webhook;
        private Presets _presets;

        public Main()
        {
            InitializeComponent();
            outputBuilder.TextChanged += OutputBuilder_TextChanged;

            foreach (var java in GetJavaInstallationPath())
            {
                JavaPathComboBox.Items.Add(java);
            }

            ChangeEnabledConfigurationButtons(false);

            if (JavaPathComboBox.Items.Count == 0)
            {
                MessageBox.Show("Any java installation was not found in your PC. If it's installed, please add to the text file \"javaLocation.txt\"",
                    "Java was not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _presets = Presets.Deserialize() ?? new();
            AddPresetsToComboBox();

            _webhook = MinecraftWebhook.LoadOrCreate();
            WebhookUrlTextBox.Text = _webhook.Url ?? "";
            DiscordWebhookCheckBox.Checked = _webhook.Enabled;
        }

        private void AddPresetsToComboBox()
        {
            for (int i = 0; i < _presets.Count; i++)
            {
                PresetsComboBox.Items.Add(_presets[i].Name);
            }
        }

        private async Task GetProcessRuntime()
        {
            while (true)
            {
                if (_process is null)
                {
                    break;
                }

                // Don't put fire on the CPU
                await Task.Delay(1000);

                ActivityTime.Invoke((MethodInvoker)delegate
                {
                    if (_process is not null)
                    {
                        var runtime = DateTime.Now - _process.StartTime;
                        ActivityTime.Text = string.Format("Activity Time: {0:hh\\:mm\\:ss}", runtime);
                    }
                });
            }
        }

        private static string GetServerFilePath(FileInfo serverFile, DirectoryInfo directory)
        {
            var pathSplitted = directory.FullName.Split(@"\");
            for (int i = 0; i < pathSplitted.Length; i++)
            {
                var folder = pathSplitted[i];

                if (folder.Contains(' '))
                {
                    folder = folder.Insert(0, "\"");
                    folder = folder.Insert(folder.Length, "\"");
                }

                pathSplitted[i] = folder;
            }

            var workingDirectory = new StringBuilder().AppendJoin(@"\\", pathSplitted).Append(@"\\").ToString();
            return (directory.FullName.Contains(' ') ? workingDirectory + serverFile.Name : serverFile.FullName);
        }

        private Task Start()
        {
            outputBuilder.AppendLine("*** Starting... ***");

            if (_serverFile is null)
            {
                MessageBox.Show("Minecraft server .jar is not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Task.CompletedTask;
            }

            if (_serverFile.Directory is null)
            {
                MessageBox.Show("Directory is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Task.CompletedTask;
            }

            var xmxNumber = string.IsNullOrWhiteSpace(MemoryAllocationTextBox.Text) ? "2" : MemoryAllocationTextBox.Text;
            var xmsNumber = string.IsNullOrWhiteSpace(MemoryAllocationTextBox.Text) ? "2" : MemoryAllocationTextBox.Text;
            var memoryArgument = $"-Xmx{xmxNumber}G -Xms{xmsNumber}G";

            // Javaw doesn't have a console window.
            var java = JavaPathComboBox.Invoke<string>(() => (string)JavaPathComboBox.SelectedItem) + "/bin/javaw.exe";
            var serverFile = GetServerFilePath(_serverFile, _serverFile.Directory);

            try
            {
                _process = new()
                {
                    StartInfo = new()
                    {
                        FileName = java,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        RedirectStandardError = true,
                        Arguments = $"{memoryArgument} -XX:+UseG1GC -jar {serverFile} nogui",
                        CreateNoWindow = true,
                        WorkingDirectory = _serverFile.DirectoryName
                    }
                };

                _process.OutputDataReceived += OutputDataReceived;
                _process.Start();

                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
                Task.Run(async () => await GetProcessRuntime());
                _process.WaitForExit();
                
            }
            catch (Exception e)
            {
                outputBuilder.AppendLine(e.ToString());
            }
            
            outputBuilder.AppendLine("*** The server has exited. ***");

            PresetsComboBox.Invoke(() => PresetsComboBox.Enabled = true);
            OpenButton.Invoke(() => OpenButton.Enabled = true);
            KillButton.Invoke(() => KillButton.Enabled = false);
            return Task.CompletedTask;
        }

        private void OutputBuilder_TextChanged(object? sender, TextEventArgs e)
        {
            OutputRichTextBox.Invoke((MethodInvoker)delegate
            {
                OutputRichTextBox.AppendText(e.Text + Environment.NewLine);
            });
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
            {
                return;
            }

            AddPlayerToListBox(e.Data);
            SendMessageWebhook(e.Data);
            
            outputBuilder.AppendLine(e.Data);
        }

        private void AddPlayerToListBox(string data)
        {
            // If doesn't have any, return.
            if ( !(data.Contains("joined") || data.Contains("left")) )
            {
                return;
            }

            var text = data;
            text = text.Remove(0, text.LastIndexOf(':') + 2);
            var split = text.Split(' ');
            var player = split[0].Trim();

            PlayersListBox.Invoke((MethodInvoker)delegate
            {
                if (data.Contains("joined"))
                    PlayersListBox.Items.Add(player);

                if (data.Contains("left"))
                    PlayersListBox.Items.Remove(player);
            });
        }

        private void SendMessageWebhook(string data)
        {
            var players = PlayersListBox.Items.OfType<string>()
                .ToImmutableList();

            var player = players.FirstOrDefault(p => data.Contains($"<{p}>"));
            if (player is null)
            {
                return;
            }

            var text = data;
            text = text.Remove(0, text.LastIndexOf('>') + 2);

            _webhook.PostAsync(player, text);
        }

        private Task Stop()
        {
            if (_process is null)
            {
                MessageBox.Show("The server is not open");
                return Task.CompletedTask;
            }

            try
            {
                PlayersListBox.Items.Clear();

                if (_process.HasExited)
                {
                    _process.Dispose();
                    _process = null;
                }
                else
                {
                    _process.Kill();
                    _process.Dispose();
                    _process = null;
                }

                outputBuilder.AppendLine("The server got killed. Any progression may have been lost!");
            }
            catch (Exception e)
            {
                outputBuilder.AppendLine(e.ToString());
            }

            PresetsComboBox.Invoke(() => PresetsComboBox.Enabled = true);
            KillButton.Invoke(() => KillButton.Enabled = false);
            OpenButton.Invoke(() => OpenButton.Enabled = true);

            return Task.CompletedTask;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (_serverFile?.Extension != ".jar")
            {
                MessageBox.Show("The file you chose is not .jar file.", "File not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (JavaPathComboBox.SelectedItem == null) 
            {
                MessageBox.Show("Select a java version to open the server", "Java not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PresetsComboBox.Enabled = false;
            KillButton.Enabled = true;
            OpenButton.Enabled = false;
            
            Task.Run(Start);
        }

        private void KillButton_Click(object sender, EventArgs e)
        {
            Task.Run(Stop);
        }

        private void clearOutput_Click(object sender, EventArgs e)
        {
            OutputRichTextBox.Clear();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SendTextBox.Text))
            {
                return;
            }

            Task.Run(() => RunCommand(SendTextBox.Text));
        }

        private void SendTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter != (Keys)e.KeyChar)
            {
                return;
            }

            e.Handled = true;
            
            Task.Run(() => RunCommand(SendTextBox.Text));
        }

        private Task RunCommand(string command)
        {
            _process?.StandardInput.WriteLine(command);
            return Task.CompletedTask;
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            if (_process is null)
            {
                return;
            }

            PlayersListBox.Items.Clear();

            Task.Run(async () => 
            {
                await RunCommand("stop");

                while(!_process.HasExited)
                {
                    await Task.Delay(500);
                }

                _process.Dispose();
                _process = null;
            });
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (_process is null)
            {
                return;
            }

            Task.Run(() => RunCommand("save-all flush"));
        }

        private void ClearCommandButton_Click(object sender, EventArgs e)
        {
            SendTextBox.Text = "";
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            var searchForms = new SearchForms(outputBuilder.GetText());
            searchForms.Show();
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                QuartzNotifyIcon.Visible = true;
                
                if (_process is not null)
                {
                    QuartzNotifyIcon.Text = "The server is open.";
                }
                else
                {
                    QuartzNotifyIcon.Text = "The server is not open.";
                }
            }
        }

        private void QuartzNotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            QuartzNotifyIcon.Visible = false;
        }

        private void OutputRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.LinkText))
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = e.LinkText,
                    UseShellExecute = true,
                });
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_process is null)
            {
                return;
            }

            var result = MessageBox.Show("The server is running, are you sure?\nIt will be killed and any progression will be lost.",
                "Close",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private async void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            _webhook.Save();

            if (_process is not null)
            {
                await Stop();
            }
        }

        private void CreatePresetButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PresetNameTextBox.Text))
            {
                MessageBox.Show("Preset name cannot be empty", "Empty name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var name = PresetNameTextBox.Text;

            if (_serverFile is null)
            {
                MessageBox.Show("Minecraft server .jar is not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            var path = _serverFile.FullName;
            var memoryAllocated = MemoryAllocationTextBox.Text ?? "2";

            if (JavaPathComboBox.SelectedItem == null)
            {
                MessageBox.Show("Java is not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var javaIndex = JavaPathComboBox.SelectedIndex;

            var server = new Server()
            {
                Name = name,
                Path = path,
                MemoryAllocated = memoryAllocated,
                JavaIndex = javaIndex
            };

            _presets.Add(server);
            _presets.Save();

            PresetsComboBox.Items.Clear();
            AddPresetsToComboBox();
        }

        private void RemovePresetButton_Click(object sender, EventArgs e)
        {
            if (PresetsComboBox.Items.Count == 0)
            {
                return;
            }

            var index = PresetsComboBox.SelectedIndex;
            var server = _presets[index];

            _presets.Remove(server);
            PresetsComboBox.Items.RemoveAt(index);

            _presets.Save();
        }

        private void PresetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = PresetsComboBox.SelectedIndex;
            var server = _presets[index];

            _serverFile = new FileInfo(server.Path);
            SelectServerJar.Text = server.Name;
            MemoryAllocationTextBox.Text = server.MemoryAllocated;
            JavaPathComboBox.SelectedIndex = server.JavaIndex;

            ChangeEnabledConfigurationButtons(true);
            OpenButton.Enabled = true;
        }

        private void DiscordWebhookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var url = WebhookUrlTextBox.Text;
            if (!url.StartsWith("https://") || !url.Contains("discord.com/api/webhooks"))
            {
                DiscordWebhookCheckBox.Checked = false;
                _webhook.Enabled = false;
                return;
            }

            Task.Run(() =>
            {
                _webhook.Enabled = DiscordWebhookCheckBox.Checked;

                if (_webhook.Url == url)
                {
                    return;
                }

                _webhook.Url = url;
            });
        }
    }
}