using System.Collections.Immutable;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Quartz;

namespace Quartz.Forms
{
    public partial class Main : Form
    {
        private Quartz _quartz;
        private MinecraftWebhook _webhook;
        private Presets _presets;
        private IList<string> _messages;
        private IList<string> _logs;
        //private TextBuilder _outputBuilder = new TextBuilder();

        public Main()
        {
            InitializeComponent();
            //outputBuilder.TextChanged += OutputBuilder_TextChanged;
            _quartz = new();
            _quartz.FindJavaInstallationsPaths();

            _quartz.PlayerJoined += OnPlayerJoined;
            _quartz.PlayerLeft += OnPlayerLeft;
            _quartz.PlayerMessage += OnPlayerMessage;
            _quartz.ProcessOutputData += OnProcessOutputData;
            _quartz.ProcessExited += OnProcessExited;

            ChangeEnabledConfigurationButtons(false);

            foreach (var java in _quartz.Javas)
            {
                JavaPathComboBox.Items.Add(java);
            }

            if (JavaPathComboBox.Items.Count == 0)
            {
                MessageBox.Show("Any java installation was not found in your PC. If it's installed, please add to the text file \"javalocation.txt\"",
                    "Java was not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _presets = Presets.Deserialize() ?? new();
            AddPresetsToComboBox();

            _webhook = MinecraftWebhook.LoadOrCreate();
            WebhookUrlTextBox.Text = _webhook.Url ?? "";
            DiscordWebhookCheckBox.Checked = _webhook.Enabled;
            
            _messages = new List<string>();
            _logs = new List<string>();
        }

        private void AppendLine(string text)
        {
            OutputRichTextBox.Invoke((MethodInvoker)delegate
            {
                OutputRichTextBox.AppendText(text + Environment.NewLine);
            });
        }

        private void AddPresetsToComboBox()
        {
            for (int i = 0; i < _presets.Count; i++)
            {
                PresetsComboBox.Items.Add(_presets[i].Name);
            }
        }

        private async Task SetupProcessRuntime()
        {
            await WaitUntil(() =>
            {
                if (!_quartz.IsOpen)
                    return true;

                ActivityTime.Invoke((MethodInvoker)delegate
                {
                    ActivityTime.Text = string.Format("Activity Time: {0:hh\\:mm\\:ss}", _quartz.Runtime);
                });

                return false;
            }, 1000, 0);

            //while (true)
            //{
            //    if (!_quartz.IsOpen)
            //    {
            //        break;
            //    }

            //    // Don't put fire on the CPU
            //    await Task.Delay(1000);

            //    ActivityTime.Invoke((MethodInvoker)delegate
            //    {
            //        if (_quartz.IsOpen)
            //        {
            //            ActivityTime.Text = string.Format("Activity Time: {0:hh\\:mm\\:ss}", _quartz.Runtime);
            //        }
            //    });
            //}
        }

        #region Start/Stop
        private async Task Start()
        {
            var memoryString = string.IsNullOrWhiteSpace(MemoryAllocationTextBox.Text) ? "2" : MemoryAllocationTextBox.Text;
            var memory = int.Parse(memoryString);

            try
            {
                await _quartz.Start(MemoryTypeFlags.Gigabytes, memory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private Task Stop()
        {
            try
            {
                _quartz.Kill();
                PlayersListBox.Items.Clear();
                AppendLine("The server got killed. Any progression may have been lost!");
            }
            catch (Exception e)
            {
                AppendLine(e.ToString());
            }

            PresetsComboBox.Invoke(() => PresetsComboBox.Enabled = true);
            KillButton.Invoke(() => KillButton.Enabled = false);
            OpenButton.Invoke(() => OpenButton.Enabled = true);

            return Task.CompletedTask;
        }
        #endregion

        #region Events
        private void OnPlayerJoined(object sender, PlayerEventArgs e)
        {
            PlayersListBox.Items.Add(e.Player);
        }

        private void OnPlayerLeft(object sender, PlayerEventArgs e)
        {
            PlayersListBox.Items.Remove(e.Player);
        }

        private void OnPlayerMessage(object sender, PlayerEventArgs e)
        {
            _messages.Add(e.Player + ": " + e.Message);
            _webhook.PostAsync(e.Player, e.Message);
        }

        private void OnProcessOutputData(object sender, string e)
        {
            _logs.Add(e);
            AppendLine(e);
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            AppendLine("*** The server has exited. ***");

            PresetsComboBox.Invoke(() => PresetsComboBox.Enabled = true);
            OpenButton.Invoke(() => OpenButton.Enabled = true);
            KillButton.Invoke(() => KillButton.Enabled = false);
        }
        #endregion

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (JavaPathComboBox.SelectedItem == null) 
            {
                MessageBox.Show("Select a java version to open the server", "Java not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PresetsComboBox.Enabled = false;
            KillButton.Enabled = true;
            OpenButton.Enabled = false;
            
            Start();
            SetupProcessRuntime();
        }

        private void KillButton_Click(object sender, EventArgs e)
        {
            Stop();
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

            _quartz.SendInput(SendTextBox.Text);
        }

        private void SendTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Keys.Enter != (Keys)e.KeyChar)
            {
                return;
            }

            e.Handled = true;

            _quartz.SendInput(SendTextBox.Text);
        }

        /// <summary>
        /// Waits to the condition to return true.
        /// 
        /// Set waitSeconds to 0, to wait indefinitely.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="millisecondsDelay"></param>
        /// <param name="waitSeconds"></param>
        /// <returns></returns>
        private async Task WaitUntil(Func<bool> condition, int millisecondsDelay = 500, int waitSeconds = 10)
        {
            var now = DateTime.Now;
            var later = now.AddSeconds(waitSeconds);

            while (true) 
            {
                if (condition.Invoke()) break;
                if (now > later)
                {
                    if (waitSeconds > 0)
                    {
                        break;
                    }
                }
                await Task.Delay(millisecondsDelay);
            }
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            if (!_quartz.IsOpen)
            {
                return;
            }

            PlayersListBox.Items.Clear();
            SaveAndCloseButton.Enabled = false;

            Task.Run(async () => 
            {
                await _quartz.SendInput("stop");
                SaveAndCloseButton.Invoke(() => SaveAndCloseButton.Text = "Waiting");

                await WaitUntil(() => !_quartz.IsOpen, waitSeconds: 0);

                SaveAndCloseButton.Invoke(() => SaveAndCloseButton.Text = "Save And Close");
                SaveAndCloseButton.Invoke(() => SaveAndCloseButton.Enabled = true);
                await _quartz.Kill();
            });
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!_quartz.IsOpen)
            {
                return;
            }

            _quartz.SendInput("save-all flush");
        }

        private void ClearCommandButton_Click(object sender, EventArgs e)
        {
            SendTextBox.Text = "";
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //var text = OutputRichTextBox.Text.Split(Environment.NewLine);
            var searchForms = new SearchForms(_logs.AsReadOnly());
            searchForms.Show();
        }

        #region Quartz Icon
        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                QuartzNotifyIcon.Visible = true;
                
                if (_quartz.IsOpen)
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
        #endregion

        #region Close Main Forms
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_quartz.IsOpen)
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

            if (_quartz.IsOpen)
            {
                await Stop();
            }
        }
        #endregion

        #region Presets
        private void CreatePresetButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PresetNameTextBox.Text))
            {
                MessageBox.Show("Preset name cannot be empty", "Empty name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var name = PresetNameTextBox.Text;

            if (_quartz.Name == "")
            {
                MessageBox.Show("Minecraft server .jar is not selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            var path = _quartz.FullName;
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

            _quartz.LoadServer(server.Path);
            SelectServerJar.Text = server.Name;
            MemoryAllocationTextBox.Text = server.MemoryAllocated;
            JavaPathComboBox.SelectedIndex = server.JavaIndex;

            ChangeEnabledConfigurationButtons(true);
            OpenButton.Enabled = true;
        }
        #endregion

        #region Misc
        private void DiscordWebhookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var url = WebhookUrlTextBox.Text;
            //if (!url.StartsWith("https://") || !url.Contains("discord.com/api/webhooks"))
            //{
            //    DiscordWebhookCheckBox.Checked = false;
            //    _webhook.Enabled = false;
            //    return;
            //}

            if (!_webhook.TrySetWebhookUrl(url)) 
            {
                DiscordWebhookCheckBox.Checked = false;
                _webhook.Enabled = false;
                return;
            }

            _webhook.Enabled = DiscordWebhookCheckBox.Checked;

            if (_webhook.Url == url)
            {
                return;
            }

            _webhook.Init();
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
        
        private void JavaPathComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _quartz.Javas.SelectedIndex = JavaPathComboBox.SelectedIndex;
        }
        #endregion

    }
}