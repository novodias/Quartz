using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Avalonia.Files
{
    abstract public class FileConfiguration
    {
        private readonly static string _dir = "configuration";
        private static string _path => Path.Combine(AppContext.BaseDirectory, _dir, Name);

        protected static string Name = "";

        protected FileConfiguration()
        {
        }

        protected static T? Load<T>(string name) where T : FileConfiguration
        {
            var path = Path.Combine(AppContext.BaseDirectory, _dir, name);

            if (!File.Exists(path))
            {
                return null;
            }

            T? result = null;
            using (var sr = new StreamReader(path))
            {
                var deserialized = sr.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(deserialized);
            }

            return result;
        }

        public void Save()
        {
            var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, _dir));

            if (!dir.Exists)
            {
                dir.Create();
            }

            using (var sw = new StreamWriter(_path))
            {
                var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
                sw.Write(serialized);
            }
        }
    }
}
