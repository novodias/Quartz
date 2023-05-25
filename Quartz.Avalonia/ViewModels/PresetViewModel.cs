using Avalonia.Media.Imaging;
using QuartzAvalonia.Files;
using QuartzAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.ViewModels
{
    public class PresetViewModel : ViewModelBase
    {
        private readonly ServerModel _model;
        
        public Server Server => _model.Server;
        
        private Bitmap? _icon;
        public Bitmap? Icon
        {
            get => _icon;
            private set => this.RaiseAndSetIfChanged(ref _icon, value);
        }

        public PresetViewModel(ServerModel model) 
        {
            _model = model;
        }

        public async Task LoadIcon()
        {
            await using (var imageStream = await _model.LoadIconBitmap())
            {
                Icon = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 32));
            }
        }

        public string Name => Server.Name;
        public string Path => Server.Path;
        public int JavaIndex => Server.JavaIndex;
        public string Memory => Server.Memory;
        public int Index => _model.Index;
    }
}
