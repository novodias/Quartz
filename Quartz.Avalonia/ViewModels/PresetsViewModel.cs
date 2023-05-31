using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaEdit.Utils;
using QuartzAvalonia.Files;
using QuartzAvalonia.Models;
using QuartzAvalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.ViewModels
{
    public class PresetsViewModel : ViewModelBase
    {
        public PresetsConfiguration PresetsConfiguration { get; set; }
        public ObservableCollection<PresetViewModel> PresetServers { get; }

        public Server? NewServer { get; }
        public Server? SelectedServer { get; private set; } = null;
        public PresetViewModel? SettingsServer { get; private set; } = null;

        private string? _presetName;
        public string? PresetName
        {
            get => _presetName;
            set => this.RaiseAndSetIfChanged(ref _presetName, value);
        }

        public PresetsViewModel()
        {
            PresetsConfiguration = PresetsConfiguration.LoadOrCreate();
            NewServer = null;
            PresetServers = new();
            LoadPresets();
        }

        public PresetsViewModel(Server? server = null)
        {
            PresetsConfiguration = PresetsConfiguration.LoadOrCreate();
            NewServer = server;
            PresetServers = new();
            LoadPresets();
        }

        public async void Load(int index)
        {
            var desktop = Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime;
            var presetWindow = desktop!.Windows.First(window => window is PresetsWindowView);
            var preset = PresetServers[index];

            if (!preset.Exists)
            {
                if (!preset.VerifyIfServerFileExists())
                {
                    await MessageBox.Show(presetWindow,
                        "Something went wrong, looks like the server file was not found",
                        "Server file not found",
                        MessageBox.MessageBoxButtons.Ok
                    );
                }
                else if (!preset.VerifyIfDirectoryExists())
                {
                    await MessageBox.Show(presetWindow,
                        "Something went wrong, looks like the directory of the server was not found",
                        "Directory not found",
                        MessageBox.MessageBoxButtons.Ok
                    );
                }
                else
                {
                    await MessageBox.Show(presetWindow,
                        "Something went wrong, looks like something is wrong to the path of the server",
                        "Path invalid",
                        MessageBox.MessageBoxButtons.Ok
                    );
                }

                return;
            }

            SelectedServer = preset.Server;
            presetWindow?.Close();
        }

        public void DeletePreset(int index)
        {
            var model = PresetServers.FirstOrDefault(m => m.Index == index);

            if (model == null)
            {
                return;
            }

            PresetServers.Remove(model);
            PresetsConfiguration.Remove(model.Server);
            PresetsConfiguration.Save();
        }

        private void LoadPresets()
        {
            int index = 0;

            var models = PresetsConfiguration.Servers.Select(s =>
            {
                var model = new PresetViewModel(new ServerModel(s, index++));
                model.LoadIcon();
                return model;
            });

            PresetServers.AddRange(models);
        }

        public void CreatePreset()
        {
            if (!NewServer.HasValue) return;
            var server = NewServer.Value;

            if (!string.IsNullOrWhiteSpace(PresetName))
            {
                var name = PresetName.Trim();
                server.Name = name;
            }

            var index = PresetServers.Count;
            var model = new PresetViewModel(new ServerModel(server, index));
            PresetServers.Add(model);
            PresetsConfiguration.Add(server);
            PresetsConfiguration.Save();

            model.LoadIcon();
        }

        public void SelectSettings(int index)
        {
            SettingsServer = PresetServers[index];
        }

        public void Dispose()
        {
            foreach (var preset in PresetServers)
            {
                preset.Dispose();
            }
        }
    }
}
