using Avalonia.Media.Imaging;
using QuartzAvalonia.Files;
using QuartzAvalonia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.ViewModels
{
    public class PresetSettingsViewModel : ViewModelBase
    {
        private Server _server;
        public PresetViewModel Preset { get; }
        public int Index { get; }
        public IList<string> JavaCollection { get; }

        public PresetSettingsViewModel(int index, PresetViewModel preset, IList<string> javaCollection)
        {
            Index = index;
            Preset = preset;
            JavaCollection = javaCollection;

            Name = Preset.Name;
            Path = Preset.Path;
            Memory = Preset.Memory;
            JavaIndex = Preset.JavaIndex;

            Preset.LoadIcon(86);
        }

        public PresetViewModel GetNewPreset()
        {
            _server.Name = Name;
            _server.Path = Path;
            _server.Memory = Memory;
            _server.JavaIndex = JavaIndex;

            return new PresetViewModel(new ServerModel(_server, Index));
        }

        public string Name { get; set; }
        public string Path { get; set; }
        public string Memory { get; set; }
        public int JavaIndex { get; set; }
        public Bitmap? Icon => Preset.Icon;

    }
}
