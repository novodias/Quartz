using System.Diagnostics;

namespace Quartz.Forms
{
    partial class Main : Form
    {
        private void ChangeEnabledConfigurationButtons(bool enabled)
        {
            AddPluginButton.Enabled = enabled;
            ServerSettingsButton.Enabled = enabled;
            OpenFolderButton.Enabled = enabled;
            JavaPathComboBox.Enabled = enabled;
            MemoryAllocationTextBox.Enabled = enabled;
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

                var serverDir = new FileInfo(openDialog.FileName);
                _quartz.LoadServer(serverDir);
                SelectServerJar.Text = _quartz.ServerFileName;
                var files = _quartz.ServerDirectory?.GetFiles();

                if (files is null || !files.Any(f => f.Name == "eula.txt"))
                {
                    var result = MessageBox.Show("Do you accept Minecraft's EULA agreement?\nMore information at: https://aka.ms/MinecraftEULA", "Minecraft EULA",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.No)
                    {
                        OpenButton.Enabled = false;
                        return;
                    }

                    File.AppendAllText(Path.Join(_quartz.ServerDirectory?.FullName, "eula.txt"), "eula=true");
                }

                ChangeEnabledConfigurationButtons(true);
                OpenButton.Enabled = true;
            }
        }

        private void ServerSettingsButton_Click(object sender, EventArgs e)
        {
            if (_quartz.ServerFileName == "")
            {
                MessageBox.Show("Please select a server .jar file");
                return;
            }

            if (_quartz.ServerDirectory is null)
            {
                MessageBox.Show("The directory of the server is not valid.");
                return;
            }

            var serverPropertiesForms = new ServerPropertiesForms(_quartz.ServerDirectory);
            serverPropertiesForms.Show();
        }

        private void AddPluginButton_Click(object sender, EventArgs e)
        {
            if (_quartz.ServerFileName == "")
            {
                MessageBox.Show("Please select a server .jar file");
                return;
            }

            var serverFiles = _quartz.ServerDirectory?.GetDirectories();

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
            if (_quartz.ServerFileName == "" || _quartz.ServerDirectory is null)
            {
                return;
            }

            var info = new ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = _quartz.ServerDirectory?.FullName
            };

            Process.Start(info);
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
