using Avalonia.Controls;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using AvaloniaEdit;
using System.Diagnostics;
using Quartz;
using System.Linq;
using Avalonia.Threading;
using AvaloniaEdit.Rendering;
using Avalonia.Collections;
using System.Collections.ObjectModel;
using QuartzAvalonia.Files;
using Quartz.Avalonia;

namespace QuartzAvalonia.Views
{
    public partial class MainWindow : Window
    {

        private readonly Quartz.Quartz Quartz;

        private IList<string> Logs { get; }
        private MinecraftWebhook Webhook { get; set; }
        private ObservableCollection<string> Players { get; }

        private void OnProcessOutputData(object? sender, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return;
            }

            AppendLine(data);
        }

        private void AppendLine(string text)
        {
            if (text is not null)
            {
                Logs.Add(text);
                Dispatcher.UIThread.Post(() =>
                {
                    Editor.AppendText(text + Environment.NewLine);

                    if (!ScrollToEndCheckBox.IsChecked.HasValue) return;

                    if (ScrollToEndCheckBox.IsChecked.Value)
                    {
                        Editor.ScrollToEnd();
                    }
                });
            }

        }

        private async Task StartServer()
        {
            static int ParseNumber(string text)
            {
                string number = "";
                foreach (var digit in text)
                {
                    if (char.IsDigit(digit))
                    {
                        number += digit;
                    }
                    else
                    {
                        break;
                    }
                }
                return int.Parse(number);
            }

            var text = MemoryTextBox.Text ?? "1";
            var memory = ParseNumber(text);
            MemoryTypeFlags memoryType;
            if (text.Contains("G"))
            {
                memoryType = MemoryTypeFlags.Gigabytes;
            } 
            else if (text.Contains("M"))
            {
                memoryType = MemoryTypeFlags.Megabytes;
            } 
            else
            {
                if (memory > 64)
                {
                    memoryType = MemoryTypeFlags.Megabytes;
                } 
                else
                {
                    memoryType = MemoryTypeFlags.Gigabytes;
                }
            }

            try
            {
                await Quartz.Start(memoryType, memory);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task StopServer()
        {
            try
            {
                Quartz.Kill();
                PlayersListBox.Items = Enumerable.Empty<string>();
                AppendLine("The server got killed. Any progression may have been lost!");
            }
            catch (Exception e)
            {
                AppendLine(e.ToString());
            }

            return Task.CompletedTask;
        }

        public async void OpenServerCommand(object? sender, RoutedEventArgs e)
        {
            var result = await OpenFile.SearchJarAsync(this);

            if (result == null)
            {
                return;
            }

            Quartz.LoadServer(result[0]);
            ServerName.Content = Quartz.Name;
        }

        public async void OpenFolderCommand(object? sender, RoutedEventArgs e)
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

            var info = new ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = Quartz.Directory!.FullName
            };

            Process.Start(info);
        }
    }
}