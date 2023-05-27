using Avalonia.Media;
using Avalonia.Media.Imaging;
using QuartzAvalonia.Files;
using QuartzAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
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

        private FileInfo? _file;
        private DirectoryInfo? _dir;

        public IBrush Brush
        {
            get
            {
                var crimsom = SolidColorBrush.Parse("#DC143C");
                var gray = SolidColorBrush.Parse("#808080");
                
                if (!VerifyIfServerFileExists())
                {
                    return crimsom;
                }

                if (!VerifyIfDirectoryExists())
                {
                    return crimsom;
                }

                return gray;
            }
        }

        public bool Exists => VerifyIfServerFileExists() && VerifyIfDirectoryExists();
        
        public PresetViewModel()
        {
            _model = null;
        }

        public PresetViewModel(ServerModel model) 
        {
            _model = model;
            _file = new(_model.Server.Path);
            _dir = _file.Directory;
        }

        public async Task LoadIcon(int width = 32)
        {
            await using (var imageStream = await _model.LoadIconBitmap())
            {
                Icon = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, width));
            }
        }

        public bool VerifyIfDirectoryExists() => _dir != null ? _dir.Exists : false;

        public bool VerifyIfServerFileExists() => _file != null ? _file.Exists : false;

        public string Name => Server.Name;
        public string Path => Server.Path;
        public int JavaIndex => Server.JavaIndex;
        public string Memory => Server.Memory;
        public int Index => _model.Index;
    }
}
