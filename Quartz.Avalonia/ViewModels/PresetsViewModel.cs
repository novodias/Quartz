using DynamicData.Binding;
using QuartzAvalonia.Files;
using QuartzAvalonia.ViewModels;
using QuartzAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using QuartzAvalonia.Views;

namespace QuartzAvalonia.ViewModels
{
    public class PresetsViewModel : ViewModelBase
    {   
        public PresetsConfiguration Presets { get; set; }
        public ObservableCollection<ServersModel> Servers { get; }

        public Server? PresetServer { get; }
        public Server? Server { get; set; }

        private string? _presetName;
        public string? PresetName 
        {
            get => _presetName;
            set => this.RaiseAndSetIfChanged(ref _presetName, value);
        }

        public PresetsViewModel()
        {
            Presets = PresetsConfiguration.LoadOrCreate();
            PresetServer = null;
            Servers = new();
            LoadPresets();
        }

        public PresetsViewModel(Server? server = null)
        {
            Presets = PresetsConfiguration.LoadOrCreate();
            PresetServer = server;
            Servers = new();
            LoadPresets();
        }

        public void Load(int index)
        {
            var server = Servers[index].Server;
            Server = server;
            
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var preset = desktop.Windows.FirstOrDefault(window => window is PresetsView);
                preset?.Close();
            }
        }

        public void DeletePreset(int index)
        {
            //var model = Servers[index];
            var model = Servers.FirstOrDefault(m => m.Index == index);

            if (model == null)
            {
                return;
            }

            Servers.Remove(model);
            Presets.Remove(model.Server);
            Presets.Save();
        }

        private void LoadPresets()
        {
            int index = 0;
            foreach (var preset in Presets.Servers)
            {
                Servers.Add(new ServersModel(preset, index++));
            }
        }

        public void CreatePreset()
        {
            if (!PresetServer.HasValue) return;
            var server = PresetServer.Value;

            if (!string.IsNullOrWhiteSpace(PresetName))
            {
                var name = PresetName.Trim();
                server.Name = name;
            }

            var index = Servers.Count;
            var model = new ServersModel(server, index);
            Servers.Add(model);
            Presets.Add(server);
            Presets.Save();
        }
    }
}
