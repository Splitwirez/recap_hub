using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;
using Avalonia;
using System.Threading.Tasks;
using System.Threading;

namespace ResurrectionCapsule.Launcher.Context
{
    public class LauncherContext : INotifyPropertyChanged
    {
        ObservableCollection<string> _serverConsoleData = new ObservableCollection<string>();
        public ObservableCollection<string> ServerConsoleData
        {
            get => _serverConsoleData;
            set
            {
                _serverConsoleData = value;
                NotifyPropertyChanged();
            }
        }

        bool _canRunGame = false;
        public bool CanRunGame
        {
            get => _canRunGame;
            set
            {
                _canRunGame = value;
                NotifyPropertyChanged();
            }
        }

        bool _needsGamePath = false;
        public bool NeedsGamePath
        {
            get => _needsGamePath;
            set
            {
                _needsGamePath = value;
                NotifyPropertyChanged();
            }
        }

        string _gameInstallPath = string.Empty;
        public string GameInstallPath
        {
            get => _gameInstallPath;
            set
            {
                _gameInstallPath = value;
                WriteSetting(nameof(GameInstallPath), GameInstallPath);
                NotifyPropertyChanged();
            }
        }

        string _gameExePath = string.Empty;
        public string GameExePath
        {
            get => _gameExePath;
            set
            {
                _gameExePath = value;
                WriteSetting(nameof(GameExePath), GameExePath);
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ResurrectionCapsule");
        
        public void LaunchGameCommand()
        {
            if (!NeedsGamePath)
            {
                if (!string.IsNullOrEmpty(GameExePath))
                {
                    Process darkspore = new Process()
                    {
                        StartInfo = new ProcessStartInfo(GameExePath),
                        EnableRaisingEvents = true
                    };
                    darkspore.Exited += (sneder, args) => CanRunGame = true;
                    try
                    {
                        darkspore.Start();
                        CanRunGame = false;
                    }
                    catch (Exception ex)
                    {
                        AddStringToData(ex.ToString());
                    }
                }
            }
        }

        void WriteSetting(string name, object value)
        {
            if (!Directory.Exists(_configPath))
                Directory.CreateDirectory(_configPath);

            string settingFile = Path.Combine(_configPath, name);
            File.WriteAllText(settingFile, value.ToString());
        }

        object ReadSetting(string name)
        {
            string settingFile = Path.Combine(_configPath, name);

            if (File.Exists(settingFile))
                return File.ReadAllText(settingFile);
            else
                return null;
        }

        bool ValidateGameInstallPath(string path)
        {
            bool valid = false;
            if (!(string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)))
                valid = Directory.Exists(path);
            if (valid)
                valid = File.Exists(Path.Combine(path, "DarksporeBin", "Darkspore.exe"));

            return valid;
        }

        bool ValidateGameExePath(string path)
        {
            if (!(string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)))
                return ValidateGameInstallPath(new FileInfo(path).Directory.Parent.ToString());
            else
                return false;
        }

        string _path = string.Empty;
        static Process _server = null;
        public LauncherContext()
        {
            //_path =  new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Directory.ToString();
            _path = Environment.CurrentDirectory;

            var startInfo = new ProcessStartInfo(Path.Combine(_path, "build", "darkspore_server.exe"))
            {
                //UseShellExecute = false,
                WorkingDirectory = Path.Combine(_path, "build")/*,
                CreateNoWindow = true*/
            };
            //startInfo.UseShellExecute = false;
            _server = new Process()
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };
            _server.StartInfo.RedirectStandardOutput = true;
            _server.StartInfo.RedirectStandardError = true;
            /*string output = server.StandardOutput.ReadToEnd();
            ServerConsoleData.Add(output);
            server.WaitForExit();*/
            _server.OutputDataReceived += Server_OutputDataReceived; //(sneder, args) => Debug.WriteLine(args.Data);
            _server.ErrorDataReceived += Server_OutputDataReceived; //(sneder, args) => Debug.WriteLine(args.Data);
            //process.WaitForExit();
            var writer = new ConsoleWriter();
            writer.WrittenTo += (sneder, data) =>
            {
                Debug.WriteLine(data);
                AddStringToData(data);
            };
            Console.SetOut(writer);
            Console.SetError(writer);
            _server.Start();
            _server.BeginOutputReadLine();
            new Thread(() =>
            {
                //Thread.CurrentThread.IsBackground = true;
                _server.WaitForExit();
            }).Start();
            //_server.BeginOutputReadLine();
            //server.BeginErrorReadLine();

            Timer consoleTimer = new Timer(1);
            consoleTimer.Elapsed += (sneder, args) =>
            {
                Debug.WriteLine("ELAPSED");
                if (false)
                {
                    //server.StandardOutput.BaseStream.Seek(0, SeekOrigin.Begin);
                    //server.StandardError.BaseStream.Seek(0, SeekOrigin.Begin);
                    /*byte[] output = new byte[server.StandardOutput.BaseStream.Length];
                    server.StandardOutput.BaseStream.Read(output, 0, (int)server.StandardOutput.BaseStream.Length);*/
                    Stream stream = new MemoryStream();
                    //.async.BaseStream.CopyTo(stream);

                    //Task<string> task = _server.StandardOutput.ReadToEndAsync(); //Task<string>.Run(, CancellationToken.None);
                    //task.Start();
                    //task.Wait();
                    //string result = ;
                    List<string> lines; // = task.Result.Split('\n').ToList(); //new StreamReader(stream).ReadToEnd().Split('\n').ToList(); //output.ToString().Split('\n').ToList();
                    //List<string> lines2 = server.StandardError.ReadToEnd().Split('\n').ToList();
                    ClearData();
                    foreach (string s in lines)
                        AddStringToData(s);
                    /*foreach (string s in lines2)
                        AddStringToData(s);*/
                }

                //GetConsoleOutput().Start();
                /*Task<string> task = _server.StandardOutput.ReadToEndAsync();
                task.RunSynchronously();
                string data = task.Result;
                AddStringToData(data);*/
                string data = _server.StandardOutput.ReadToEnd();
                Debug.WriteLine(data);
                AddStringToData(data);
            };
            //consoleTimer.Start();
            if (false)
            {
                while (!_server.HasExited/*.StandardOutput.EndOfStream*/)
                {
                    string data = _server.StandardOutput.ReadLine();
                    if (!((string.IsNullOrEmpty(data)) || string.IsNullOrWhiteSpace(data)))
                    {
                        Debug.WriteLine(data);
                        AddStringToData(data);
                    }
                }
            }
            //Console.WriteLine("data");

            string exePathFile = Path.Combine(_configPath, nameof(GameExePath));
            string exePath = string.Empty;
            if (File.Exists(exePathFile))
                exePath = File.ReadAllText(exePathFile);

            if (!ValidateGameExePath(exePath))
            {
                Debug.WriteLine("initial game install validation failed!");
                NeedsGamePath = true;
                Timer timer = new Timer(100);
                timer.Elapsed += (sneder, args) =>
                {
                    Process[] processes = Process.GetProcesses();
                    //.Where(x => Path.GetFileName(x.MainModule.FileName).ToLowerInvariant().Contains("darkspore") && (!Path.GetFileName(x.MainModule.FileName).ToLowerInvariant().Contains("server")))
                    Process process = null;
                    for (int i = 0; i < processes.Length; i++)
                    {
                        Process p = processes[i];
                        try
                        {
                            string exe = GetExecutablePath(p).ToLowerInvariant();
                            if (exe.Contains("darkspore") && exe.EndsWith(".exe") && (!exe.Contains("server")))
                            {
                                process = p;
                                break;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    /*.Where(x =>
                    {
                        string exe = Path.GetFileName(x.MainModule.FileName).ToLowerInvariant();
                        return exe.Contains("darkspore") && exe.EndsWith(".exe") && (!exe.Contains("server"));
                    });*/ //.GetProcessesByName("darkspore");
                    if (process != null)
                    {
                        string fileName = GetExecutablePath(process);
                        if (ValidateGameExePath(fileName))
                        {
                            GameExePath = fileName;
                            GameInstallPath = new FileInfo(fileName).Directory.Parent.ToString();
                            NeedsGamePath = false;
                            process.Kill();
                            timer.Stop();
                        }
                    }
                };
                timer.Start();
            }
        }

        public async Task GetConsoleOutput()
        {
            ClearData();
            AddStringToData(await _server.StandardOutput.ReadToEndAsync());
        }

        void ClearData()
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => ServerConsoleData.Clear());
        }

        void AddStringToData(string data)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => ServerConsoleData.Add(data));
        }

        private void Server_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddStringToData(e.Data);
            Debug.WriteLine("DATA: " + e.Data);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(int flags, bool inherit, int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, out int lpdwSize);

        public static string GetExecutablePath(Process process)
        {
            string returnValue = string.Empty;
            StringBuilder stringBuilder = new StringBuilder(1024);
            IntPtr hprocess = OpenProcess(0x1000, false, process.Id);

            if (hprocess != IntPtr.Zero)
            {
                int size = stringBuilder.Capacity;

                if (QueryFullProcessImageName(hprocess, 0, stringBuilder, out size))
                    returnValue = stringBuilder.ToString();
            }
            return returnValue;
        }
    }
}
