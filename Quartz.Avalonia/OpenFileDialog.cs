using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Avalonia
{
    public static class OpenFile
    {
        public static async Task<string[]?> SearchJarAsync(Window parent, bool allowMultiple = false)
        {
            var window = new OpenFileDialog();
            window.Title = "Select your jar file";
            window.Directory = AppContext.BaseDirectory;
            window.Filters = new List<FileDialogFilter>()
            {
                new FileDialogFilter() { Name = "Jar", Extensions = { "jar" }}
            };
            window.AllowMultiple = allowMultiple;

            var result = await window.ShowAsync(parent);

            if (result is null)
            {
                return null;
            }

            return result;
        }
    }
}
