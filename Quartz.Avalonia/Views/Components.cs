﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using Quartz;
using QuartzAvalonia.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuartzAvalonia.Files;
using Discord;
using QuartzAvalonia.ViewModels;
using Quartz.Avalonia;

namespace QuartzAvalonia.Views
{
    public partial class MainWindow : Window
    {
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

        public MainWindow()
        {
            InitializeComponent();

            Editor.Text = "";
            Logs = new List<string>();

            Quartz = new();
            Quartz.FindJavaInstallationsPaths();
            Quartz.ProcessOutputData += OnProcessOutputData;
            Quartz.PlayerJoined += OnPlayerJoined;
            Quartz.PlayerLeft += OnPlayerLeft;
            Quartz.ProcessExited += OnProcessExited;
            Quartz.ProcessStarted += OnProcessStarted;
            Quartz.PlayerMessage += OnPlayerMessage;
            JavaComboBox.Items = Quartz.Javas;


            Players = new ObservableCollection<string>();
            PlayersListBox.Items = Players;

            AddEvents();

            // Enable it upon the server start
            Send.IsEnabled = false;
            Clear.IsEnabled = false;

            Webhook = MinecraftWebhook.LoadOrCreate();

            if (!string.IsNullOrWhiteSpace(Webhook.Url))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    DiscordTextBox.Text = Webhook.Url;
                    DiscordCheckBox.IsChecked = Webhook.Enabled;
                });
                Webhook.Init();
            }
            else
            {
                DiscordCheckBox.IsChecked = false;
            }
        }

        private void OnPlayerMessage(object? sender, PlayerEventArgs e)
        {
            #pragma warning disable CS4014
            Webhook.PostAsync(e.Player, e.Message);
        }

        private void OnProcessStarted(object? sender, EventArgs e)
        {
            SetupProcessRuntime();
            #pragma warning restore
            ServerStatus.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#1cac39"));
            Kill.Background = SolidColorBrush.Parse("#DC143C");
            PresetsButton.IsEnabled = false;
            Start.IsEnabled = false;
            Open.IsEnabled = false;
            Kill.IsEnabled = true;
            Send.IsEnabled = true;
            Clear.IsEnabled = true;
        }

        private void OnProcessExited(object? sender, EventArgs e)
        {
            ServerStatus.Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#DC143C"));
            Kill.Background = SolidColorBrush.Parse("#808080");
            Send.IsEnabled = false;
            Clear.IsEnabled = false;
            Kill.IsEnabled = false;
            Open.IsEnabled = true;
            Start.IsEnabled = true;
            PresetsButton.IsEnabled = true;
        }

        public void OnPlayerJoined(object? sender, Quartz.PlayerEventArgs e)
        {
            Players.Add(e.Player);
        }

        public void OnPlayerLeft(object? sender, Quartz.PlayerEventArgs e)
        {
            Players.Remove(e.Player);
        }

        public void AddEvents()
        {
            Open.Click += OpenServerCommand;
            Folder.Click += OpenFolderCommand;
            Settings.Click += SettingsButton_OnClick;
            AddPlugin.Click += AddPluginButton_OnClick;

            PresetsButton.Click += PresetsButton_Click;
            Start.Click += StartButton_OnClick;
            Kill.Click += KillButton_OnClick;
            
            Send.Click += SendButton_OnClick;
            Clear.Click += ClearButton_OnClick;

            JavaComboBox.SelectionChanged += OnSelectionChanged;

            CommandTextBox.KeyUp += CommandTextBox_KeyUp;

            //DiscordCheckBox.Checked += DiscordCheckBox_Checked;
            //DiscordCheckBox.PointerReleased += DiscordCheckBox_Checked;
            DiscordCheckBox.Click += DiscordCheckBox_Checked;
            DiscordTextBox.AddHandler(TextInputEvent, DiscordTextBox_TextInput, RoutingStrategies.Tunnel);

            Closing += OnClosing;
        }

        private async void PresetsButton_Click(object? sender, RoutedEventArgs e)
        {
            Server? preset = null;

            if (Quartz.IsLoaded)
            {
                if (JavaComboBox.SelectedIndex != -1)
                {
                    preset = new Server()
                    {
                        Name = Quartz.Name,
                        Path = Quartz.FullName,
                        Memory = MemoryTextBox.Text,
                        JavaIndex = JavaComboBox.SelectedIndex
                    };
                }
            }

            var server = await PresetsWindowView.ShowPresets(this, Quartz.Javas, preset);
            if (server != null) 
            {
                Quartz.LoadServer(server.Value.Path);
                ServerName.Content = server.Value.Name;
                MemoryTextBox.Text = server.Value.Memory;
                JavaComboBox.SelectedIndex = server.Value.JavaIndex;
            }
        }

        private void DiscordTextBox_TextInput(object? sender, Avalonia.Input.TextInputEventArgs e)
        {
            DiscordCheckBox.IsChecked = Webhook.Enabled = false;
        }

        private void DiscordCheckBox_Checked(object? sender, RoutedEventArgs e)
        {
            var url = DiscordTextBox.Text;

            Webhook.Enabled = DiscordCheckBox.IsChecked!.Value;

            if (Webhook.Url == url)
            {
                return;
            }

            if (!Webhook.TrySetWebhookUrl(url))
            {
                DiscordCheckBox.IsChecked = Webhook.Enabled = false;
                return;
            }

            Webhook.Init();
        }

        private async void OnClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;

            if (!Quartz.IsOpen)
            {
                e.Cancel = false;
                Webhook.Save();
            }

            var result = await MessageBox.Show(this,
                "Your server is still open, do you want to kill it? Notice that this may cause lost progress.",
                "Server still open",
                MessageBox.MessageBoxButtons.OkCancel
            );

            if (result == MessageBox.MessageBoxResult.Ok)
            {
                await StopServer();
                Webhook.Save();
                e.Cancel = false;
            }
        }

        private async Task SetupProcessRuntime()
        {
            await WaitUntil(() =>
            {
                if (!Quartz.IsOpen)
                    return true;

                if (Quartz.Runtime.Days > 0)
                {
                    ServerRuntime.Content = string.Format("Runtime: {0:\\[d'.'\\]hh\\:mm\\:ss}", Quartz.Runtime);
                } 
                else
                {
                    ServerRuntime.Content = string.Format("Runtime: {0:hh\\:mm\\:ss}", Quartz.Runtime);
                }

                return false;
            }, 1000, 0);
        }

        private void CommandTextBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                var input = CommandTextBox.Text;
                Quartz.SendInput(input);
            }
        }

        public void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            Quartz.Javas.SelectedIndex = JavaComboBox.SelectedIndex;
        }

        public async void SettingsButton_OnClick(object? sender, RoutedEventArgs e)
        {
            await MessageBox.Show(this, 
                "Settings is incomplete", 
                "Settings",
                MessageBox.MessageBoxButtons.Ok
            );
        }
        public async void AddPluginButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (!Quartz.IsLoaded)
            {
                await MessageBox.Show(this, 
                    "The server is not selected, please select the server and try again.", 
                    "Server not selected", 
                    MessageBox.MessageBoxButtons.Ok
                );
                return;
            }

            var directories = Quartz.Directory!.GetDirectories();
            if (directories is null || directories.Length == 0)
            {
                await MessageBox.Show(this,
                    "Start the server for a few seconds to create folders and files and try again.",
                    "Server not started",
                    MessageBox.MessageBoxButtons.Ok
                );
                return;
            }

            var pluginFolder = directories.FirstOrDefault(d => d.Name.ToLower() == "plugins");
            if (pluginFolder == null)
            {
                await MessageBox.Show(this,
                    "There is no plugins folder, make sure your server supports plugins.",
                    "Plugins folder not available",
                    MessageBox.MessageBoxButtons.Ok
                );
                return;
            }

            var result = await OpenFile.SearchJarAsync(this, true);

            if (result is null) 
            {
                return;
            }

            foreach (var name in result)
            {
                var plugin = new FileInfo(name);
                plugin.MoveTo(Path.Join(pluginFolder.FullName, plugin.Name));
            }
        }

        public async void StartButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (!Quartz.IsLoaded)
            {
                await MessageBox.Show(this, 
                    "The server is not selected, " +
                    "please select the server and try again.", 
                    "Server not selected", 
                    MessageBox.MessageBoxButtons.Ok
                );
                return;
            }

            if (JavaComboBox.SelectedItem == null)
            {
                await MessageBox.Show(this,
                    "The java runtime is not selected, " +
                    "please select and try again.",
                    "Server not selected",
                    MessageBox.MessageBoxButtons.Ok
                );
                return;
            }

            #pragma warning disable CS4014
            StartServer();
        }

        public async void KillButton_OnClick(object? sender, RoutedEventArgs e)
        {
            if (!Quartz.IsOpen)
            {
                await MessageBox.Show(this, "The server is not open.", "Server not open", MessageBox.MessageBoxButtons.Ok);
                return;
            }

            StopServer();
            #pragma warning restore
        }

        public void SendButton_OnClick(object? sender, RoutedEventArgs e)
        {
            var input = CommandTextBox.Text;
            Quartz.SendInput(input);
        }

        public void ClearButton_OnClick(object? sender, RoutedEventArgs e)
        {
            CommandTextBox.Text = "";
        }
    }
}
