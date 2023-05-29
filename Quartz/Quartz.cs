using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Quartz;

public class Quartz : IDisposable
{
    #region Private Properties
    private bool _disposed = false;
    private Process? _process = null;
    private FileInfo? _server = null;
    private readonly IList<string> _players;
    #endregion

    #region Getters
    public bool IsOpen
    {
        get
        {
            if (_process is null)
            {
                return false;
            }

            if (_process.HasExited)
            {
                return false;
            }

            return true;
        }
    }
    public bool IsLoaded => Name != "";

    public string Name => _server?.Name ?? "";
    public string FullName => _server?.FullName ?? "";
    public DirectoryInfo? Directory => _server?.Directory;
    public TimeSpan Runtime => (DateTime.Now - _process?.StartTime) 
        ?? TimeSpan.Zero;
    #endregion

    #region Lists
    public IReadOnlyList<string> Players => (IReadOnlyList<string>)_players;
    public readonly JavaList Javas;
    #endregion

    #region Events Properties
    // Player Related
    public event EventHandler<PlayerEventArgs>? PlayerJoined = null;
    public event EventHandler<PlayerEventArgs>? PlayerLeft = null;
    public event EventHandler<PlayerEventArgs>? PlayerMessage = null;

    // Process Related
    public event EventHandler<string>? ProcessOutputData = null;
    public event EventHandler<EventArgs>? ProcessExited = null;
    public event EventHandler<EventArgs>? ProcessStarted = null;
    #endregion

    public Quartz()
    {
        _players = new List<string>();

        Javas = new();
    }

    public void LoadServer(string path)
    {
        var file = new FileInfo(path);

        if (!file.Exists)
        {
            throw new Exception("Server file doesn't exist");
        }

        if (file.Extension != ".jar")
        {
            throw new Exception("Server extension is not .jar");
        }

        _server = file;
    }

    public void LoadServer(FileInfo file)
    {
        if (!file.Exists)
        {
            throw new Exception("Server file doesn't exist");
        }

        if (file.Extension != ".jar")
        {
            throw new Exception("Server extension is not .jar");
        }

        _server = file;
    }

    public static string GetJavaRegistryValue(string directory)
    {
        //string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
        
        string javaKey = $@"SOFTWARE\JavaSoft\{directory}";

        using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
        {
            if (rk is null)
            {
                return "";
            }

            string currentVersion = rk.GetValue("CurrentVersion").ToString();
            using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
            {
                return key.GetValue("JavaHome").ToString();
            }
        }
    }

    public void FindJavaInstallationsPaths()
    {
        var baseDir = new DirectoryInfo(AppContext.BaseDirectory);
        var files = baseDir.GetFiles();
        var javasFile = files.FirstOrDefault(f => f.Name == "javalocations.txt");

        if (javasFile != null)
        {
            var locations = File.ReadAllLines(javasFile.FullName).ToList();
            locations.RemoveAll(l => l.StartsWith('#'));
            Javas.AddRange(locations);
            return;
        }

        var textInfo = "# Each line is a path to a java directory, if your java has not found, please insert here.\n" +
            "# Delete the file if the paths gets deleted to reset to default ones.";

        List<string> javas = new()
        {
            textInfo
        };

        string? environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (!string.IsNullOrEmpty(environmentPath))
        {
            javas.Add(environmentPath);
        }

        //string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
        //using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
        //{
        //    string currentVersion = rk.GetValue("CurrentVersion").ToString();
        //    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
        //    {
        //        javas.Add(key.GetValue("JavaHome").ToString());
        //    }
        //}

        //string JDK = "SOFTWARE\\JavaSoft\\JDK\\";
        //using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(JDK))
        //{
        //    string currentVersion = rk.GetValue("CurrentVersion").ToString();
        //    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
        //    {
        //        javas.Add(key.GetValue("JavaHome").ToString());
        //    }
        //}

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var javaRuntime = GetJavaRegistryValue("Java Runtime Environment");
            var jdk = GetJavaRegistryValue("JDK");
        
            if (!string.IsNullOrEmpty(javaRuntime))
            {
                javas.Add(javaRuntime);
            }

            if (!string.IsNullOrEmpty(jdk))
            {
                javas.Add(jdk);
            }
        }

        javasFile = new(Path.Join(baseDir.FullName, "javalocations.txt"));

        using var sw = javasFile.CreateText();
        foreach (var text in javas)
        {
            sw.Write(text);
        }

        javas.Remove(textInfo);
        Javas.AddRange(javas);
    }

    public async Task Start(MemoryTypeFlags memoryType, int memoryAllocation)
    {
        if (memoryAllocation <= 0)
            throw new Exception("Memory allocation cannot be less or equal to 0");
        if (_server == null) 
            throw new NullReferenceException("FileInfo server is null");
        if (_server.Directory == null)
            throw new NullReferenceException("Server directory is null");
        if (!Javas.Any())
            throw new Exception("There isn't any java installations found");

        var memory = memoryType == MemoryTypeFlags.Megabytes ? "M" : "G";
        var memoryArgument = $"-Xmx{memoryAllocation}{memory} -Xms{memoryAllocation}{memory}";
        
        var java = Javas.SelectedItem + "/bin/java.exe";

        var server = GetServerFilePath(_server, _server.Directory);

        var startInfo = new ProcessStartInfo()
        {
            FileName = java,
            WorkingDirectory = _server.DirectoryName,
            Arguments = $"{memoryArgument} -XX:+UseG1GC -jar {server} nogui",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            RedirectStandardError = true,
        };

        _process = new()
        {
            StartInfo = startInfo,
        };

        _process.Start();
        ProcessStarted?.Invoke(this, new EventArgs());
        _process.OutputDataReceived += OnOutputDataReceived;
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
        await _process.WaitForExitAsync().ConfigureAwait(false);

        _process?.Dispose();
        _process = null;
                
        _players.Clear();

        ProcessExited?.Invoke(this, new EventArgs());
    }

    public Task Kill()
    {
        if (_process is null)
        {
            return Task.CompletedTask;
        }

        if (_process.HasExited)
        {
            _process.Dispose();
            _process = null;
        }
        else
        {
            _process.Kill();
            _process.Dispose();
            _process = null;
        }

        if (_process == null)
        {
            ProcessExited?.Invoke(this, new EventArgs());
        }

        _players.Clear();
        return Task.CompletedTask;
    }

    public Task SendInput(string text)
    {
        if (_process is null)
        {
            return Task.CompletedTask;
        }

        _process.StandardInput.WriteLine(text);
        return Task.CompletedTask;
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.Data))
        {
            return;
        }

        CheckPlayerJoinedOrLeft(e.Data);
        CheckPlayerMessage(e.Data);

        ProcessOutputData?.Invoke(this, e.Data);
    }

    private void CheckPlayerMessage(string data)
    {
        var player = Players.FirstOrDefault(p => data.Contains($"<{p}>"));
        
        if (player is null)
        {
            return;
        }

        var text = data;
        var message = text.Remove(0, text.LastIndexOf('>') + 2);

        PlayerMessage?.Invoke(this, new PlayerEventArgs(player, message));
    }

    private void CheckPlayerJoinedOrLeft(string data)
    {
        if (!(data.Contains("joined") 
            || data.Contains("left")))
        {
            return;
        }

        //var text = data;
        //text = text.Remove(0, text.LastIndexOf(':') + 2);
        //var split = text.Split(' ');
        //var player = split[0].Trim();

        var split = data.Split(' ');
        var player = string.Empty;

        for (int i = 0; i < split.Length; i++)
        {
            var item = split[i].ToLower();
            if (item == "joined" || item == "left")
            {
                player = split[i - 1];
            }
        }

        if (string.IsNullOrEmpty(player)) return;

        var playerEventArgs = new PlayerEventArgs(player);

        if (data.Contains("joined"))
        {
            PlayerJoined?.Invoke(this, playerEventArgs);
            _players.Add(player);
        }
        else if (data.Contains("left"))
        {
            PlayerLeft?.Invoke(this, playerEventArgs);
            _players.Remove(player);
        }
    }

    private static string GetServerFilePath(FileInfo serverFile, DirectoryInfo directory)
    {
        var pathSplitted = directory.FullName.Split(@"\");
        for (int i = 0; i < pathSplitted.Length; i++)
        {
            var folder = pathSplitted[i];

            if (folder.Contains(' '))
            {
                folder = folder.Insert(0, "\"");
                folder = folder.Insert(folder.Length, "\"");
            }

            pathSplitted[i] = folder;
        }

        var workingDirectory = new StringBuilder().AppendJoin(@"\\", pathSplitted).Append(@"\\").ToString();
        return (directory.FullName.Contains(' ') ? workingDirectory + serverFile.Name : serverFile.FullName);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Kill();
            _players.Clear();
            _server = null;
            PlayerLeft = null;
            PlayerJoined = null;
            PlayerMessage = null;
            ProcessOutputData = null;
        }
            
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Quartz()
    {
        Dispose(false);
    }
}

public class PlayerEventArgs : EventArgs
{
    public readonly string Player;
    public readonly string? Message;

    public PlayerEventArgs(string name)
    {
        Player = name;
        Message = null;
    }

    public PlayerEventArgs(string name, string message)
    {
        Player = name;
        Message = message;
    }
}

public enum MemoryTypeFlags
{
    Megabytes,
    Gigabytes
}
