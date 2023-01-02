using System.Diagnostics;

namespace Quartz
{
    partial class Main : Form
    {
        private FileInfo? _serverFile = null;

        private void ChangeEnabledConfigurationButtons(bool enabled)
        {
            AddPluginButton.Enabled = enabled;
            ServerSettingsButton.Enabled = enabled;
            OpenFolderButton.Enabled = enabled;
            JavaPathComboBox.Enabled = enabled;
            MemoryAllocationTextBox.Enabled = enabled;
        }

        private IReadOnlyList<string> GetJavaInstallationPath()
        {
            var baseDir = new DirectoryInfo(AppContext.BaseDirectory);
            var files = baseDir.GetFiles();
            var javasFile = files.FirstOrDefault(f => f.Name == "javaLocation.txt");

            if (javasFile != null) 
            {
                var locations = File.ReadAllLines(javasFile.FullName).ToList();
                locations.RemoveAll(l => l.StartsWith('#'));
                return locations;
            }

            var textInfo = "# Each line is a path to a java directory, if your java has not found, please insert here.\n" + 
                "# Delete the file if the paths gets deleted to reset to default ones.";
            
            List<string> javas = new() 
            { 
                textInfo 
            };

            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                javas.Add(environmentPath);
            }

            string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
            {
                string currentVersion = rk.GetValue("CurrentVersion").ToString();
                using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                {
                    javas.Add(key.GetValue("JavaHome").ToString());
                }
            }

            string JDK = "SOFTWARE\\JavaSoft\\JDK\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(JDK))
            {
                string currentVersion = rk.GetValue("CurrentVersion").ToString();
                using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                {
                    javas.Add(key.GetValue("JavaHome").ToString());
                }
            }

            javasFile = new(Path.Join(baseDir.FullName, "javaLocation.txt"));
            
            using var sw = javasFile.CreateText();
            foreach (var text in javas)
            {
                sw.WriteLine(text);
            }

            javas.Remove(textInfo);

            return javas;
        }

        private void SelectServerJar_Click(object sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog();

            openDialog.Title = "Select the minecraft server jar";
            openDialog.InitialDirectory = AppContext.BaseDirectory;
            openDialog.Filter = "Jar files (*.jar)|*.jar|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                if (!openDialog.FileName.EndsWith(".jar"))
                {
                    MessageBox.Show("The file doesn't seems to be a .jar file.");
                    return;
                }

                if (openDialog.FileName.Contains(' '))
                {
                    MessageBox.Show("The path contains spaces, you may encounter bugs.\nTo resolve around this issue, rename the directories to not have a space digit", 
                        "Warning", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Exclamation);
                }
                
                _serverFile = new FileInfo(openDialog.FileName);
                SelectServerJar.Text = _serverFile.Name;
                var files = _serverFile.Directory?.GetFiles();

                if (files is null || !files.Any(f => f.Name == "eula.txt"))
                {
                    var result = MessageBox.Show("Do you accept Minecraft's EULA agreement?\nMore information at: https://aka.ms/MinecraftEULA", "Minecraft EULA",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.No)
                    {
                        OpenButton.Enabled = false;
                        return;
                    }

                    File.AppendAllText(Path.Join(_serverFile.DirectoryName, "eula.txt"), "eula=true");
                }

                ChangeEnabledConfigurationButtons(true);
                OpenButton.Enabled = true;
            }
        }

        private void ServerSettingsButton_Click(object sender, EventArgs e)
        {
            if (_serverFile is null)
            {
                MessageBox.Show("Please select a server .jar file");
                return;
            }

            if (_serverFile.Directory is null)
            {
                MessageBox.Show("The directory of the server is not valid.");
                return;
            }

            var serverPropertiesForms = new ServerPropertiesForms(_serverFile.Directory);
            serverPropertiesForms.Show();
        }

        private void AddPluginButton_Click(object sender, EventArgs e)
        {
            if (_serverFile is null)
            {
                MessageBox.Show("Please select a server .jar file");
                return;
            }

            var serverFiles = _serverFile.Directory?.GetDirectories();

            if (serverFiles is null || serverFiles.Length == 0)
            {
                MessageBox.Show("Please open the server, wait a few seconds, and close it.");
                return;
            }

            var pluginFolder = serverFiles.FirstOrDefault(d => d.Name.ToLower() == "plugins");
            if (pluginFolder == null)
            {
                MessageBox.Show("Looks like your server doesn't have support for plugins.", "Minecraft Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var openDialog = new OpenFileDialog();

            openDialog.Title = "Select the plugin jar";
            openDialog.InitialDirectory = AppContext.BaseDirectory;
            openDialog.Filter = "Jar files (*.jar)|*.jar|Zip files (*.zip)|*.zip|Rar files (.rar)|*.rar|All files (*.*)|*.*";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var files = new FileInfo[openDialog.FileNames.Length];

                int index = 0;
                foreach (var name in openDialog.FileNames)
                {
                    files[index] = new FileInfo(name);
                    index++;
                }

                foreach (var plugin in files)
                {
                    plugin.MoveTo(Path.Join(pluginFolder.FullName, plugin.Name));
                }
            }
        }

        private void OpenFolderButton_Click(object sender, EventArgs e)
        {
            if (_serverFile is null || _serverFile.Directory is null)
            {
                return;
            }

            var info = new ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = _serverFile.Directory.FullName
            };

            Process.Start(info);
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
