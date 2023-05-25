using Avalonia.Media.Imaging;
using QuartzAvalonia.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzAvalonia.Models
{
    public class ServerModel
    {
        public Server Server { get; set; }
        public int Index { get; private set; }
        private static string DefaultIcon { get; } = AppContext.BaseDirectory + "/data/MinecraftIcon_128x128.png";

        public ServerModel(Server server, int index)
        {
            Server = server;
            Index = index;
        }

        public Task<Stream> LoadIconBitmap()
        {
            var tcs = new TaskCompletionSource<Stream>();
            var icon = GetIcon(Server);
            var stream = File.OpenRead(icon);
            tcs.TrySetResult(stream);
            return tcs.Task;
        }

        public static string GetIcon(Server server)
        {
            var dir = new FileInfo(server.Path).Directory;

            if (dir == null || !dir.Exists)
            {
                return DefaultIcon;
            }

            static bool MatchExtension(string extension)
            {
                if (extension == ".jpg" ||
                    extension == ".png" ||
                    extension == ".webp" ||
                    extension == ".jpeg" ||
                    extension == ".ico")
                {
                    return true;
                }

                return false;
            }

            var file = dir.GetFiles().FirstOrDefault(f => MatchExtension(f.Extension));

            return file != null ? file.FullName : DefaultIcon;
        }
    }
}
