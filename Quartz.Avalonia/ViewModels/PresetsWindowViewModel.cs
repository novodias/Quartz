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
    public class PresetsWindowViewModel : ViewModelBase
    {   
        public PresetsConfiguration Presets { get; set; }
        public ObservableCollection<PresetViewModel> PresetServers { get; }

        public Server? NewServer { get; }
        public Server? SelectedServer { get; set; }

        private string? _presetName;
        public string? PresetName 
        {
            get => _presetName;
            set => this.RaiseAndSetIfChanged(ref _presetName, value);
        }

        public PresetsWindowViewModel()
        {
            Presets = PresetsConfiguration.LoadOrCreate();
            NewServer = null;
            PresetServers = new();
            LoadPresets();
        }

        public PresetsWindowViewModel(Server? server = null)
        {
            Presets = PresetsConfiguration.LoadOrCreate();
            NewServer = server;
            PresetServers = new();
            LoadPresets();
        }

        public void Load(int index)
        {
            var server = PresetServers[index].Server;
            SelectedServer = server;
            
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var preset = desktop.Windows.FirstOrDefault(window => window is PresetsWindow);
                preset?.Close();
            }
        }

        public void DeletePreset(int index)
        {
            //var model = Servers[index];
            var model = PresetServers.FirstOrDefault(m => m.Index == index);

            if (model == null)
            {
                return;
            }

            PresetServers.Remove(model);
            Presets.Remove(model.Server);
            Presets.Save();
        }

        private void LoadPresets()
        {
            int index = 0;
            foreach (var preset in Presets.Servers)
            {
                var presetModel = new PresetViewModel(new ServerModel(preset, index++));
                PresetServers.Add(presetModel);
            }

            foreach (var preset in PresetServers)
            {
                preset.LoadIcon();
            }
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
            Presets.Add(server);
            Presets.Save();

            model.LoadIcon();
        }
    }
}
