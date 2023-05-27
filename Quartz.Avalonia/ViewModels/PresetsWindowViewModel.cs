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
using Avalonia.Controls;
using AvaloniaEdit.Utils;
using Quartz.Avalonia;

namespace QuartzAvalonia.ViewModels
{
    public class PresetsWindowViewModel : ViewModelBase
    {
        public Server? SelectedServer { get => Presets.SelectedServer; }
        private readonly IList<string> JavaCollection;

        public PresetsWindowViewModel()
        {
            Content = Presets = new PresetsViewModel();
            JavaCollection = new List<string>();
        }

        public PresetsWindowViewModel(IList<string> javaCollection, Server? newServer = null) 
        {
            Content = Presets = new PresetsViewModel(newServer);
            JavaCollection = javaCollection;
        }


        ViewModelBase content;
        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public PresetsViewModel Presets { get; }

        public void Load(int index) => Presets.Load(index);
        public void DeletePreset(int index) => Presets.DeletePreset(index);
        public void SelectSettings(int index)
        {
            Presets.SelectSettings(index);

            Content = PresetSettings = new PresetSettingsViewModel(index, Presets.SettingsServer!, JavaCollection);
        }

        public PresetSettingsViewModel PresetSettings { get; set; }
        public void SaveSettingsPreset()
        {
            var preset = PresetSettings.GetNewPreset();
            var presetIndex = PresetSettings.Index;

            Presets.PresetsConfiguration.SetServer(presetIndex, preset.Server);
            Presets.PresetServers[presetIndex] = preset;
            Presets.PresetServers[presetIndex].LoadIcon();
            Presets.PresetsConfiguration.Save();
        }

        public void SetPresetsView()
        {
            Content = Presets;
        }

        public async void SetPath()
        {
            var desktop = (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;
            var window = desktop.Windows.First(w => w is PresetsWindowView);
            var result = await OpenFile.SearchJarAsync(window);
            PresetSettings.Path = result != null ? result[0] : PresetSettings.Path;
        }
    }
}
