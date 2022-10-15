using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ReCap.Hub.ViewModels
{
    public class LocateDarksporeViewModel : ViewModelBase, IDialogViewModel<string>
    {
        TaskCompletionSource<string> _tcs = new TaskCompletionSource<string>();
        public TaskCompletionSource<string> GetCompletionSource()
        {
            return _tcs;
        }

        string _gamePath = null;
        public string GamePath
        {
            get => _gamePath;
            set
            {
                RASIC(ref _gamePath, value);
                HasGamePath = !(string.IsNullOrEmpty(_gamePath) || string.IsNullOrWhiteSpace(_gamePath));
            }
        }


        bool _browseForGamePath = false;
        public bool BrowseForGamePath
        {
            get => _browseForGamePath;
            set => RASIC(ref _browseForGamePath, value);
        }


        bool _hasGamePath = false;
        public bool HasGamePath
        {
            get => _hasGamePath;
            set => RASIC(ref _hasGamePath, value);
        }


        bool _allowsCancel = false;
        public bool AllowsCancel
        {
            get => _allowsCancel;
            set => RASIC(ref _allowsCancel, value);
        }


        string _savePath = string.Empty;
        Timer _darksporeProcessTimer = null;
        public LocateDarksporeViewModel(bool allowsCancel = false)
        {
            AllowsCancel = allowsCancel;

            _darksporeProcessTimer = new Timer(1000);
            _darksporeProcessTimer.Elapsed += DarksporeProcessTimer_Elapsed;
            _darksporeProcessTimer.Start();
        }

        private void DarksporeProcessTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExamineProcesses();
        }

        void ExamineProcesses()
        {
            var processes = Process.GetProcesses()
                //.GetProcessesByName("darkspore*.exe")
                .ToList();

            foreach (Process process in processes)
            {
                if (TryGetDarksporeInstallPathFromProcess(process, out string winePrefix, out string wineExecutable, out string darksporeInstallPath))
                {
                    GamePath = darksporeInstallPath;
                    Accept();
                }
            }
        }

        static bool _showMsgBoxes = true;
        bool TryGetDarksporeInstallPathFromProcess(Process process, out string winePrefix, out string wineExecutable, out string darksporeInstallPath)
        {
            winePrefix = null;
            wineExecutable = null;
            darksporeInstallPath = null;
            if (process == null)
                return false;
            
            string exePath = null;
            if (OperatingSystem.IsWindows())
                exePath = Windows_GetDarksporeInstallPathFromProcess(process);
            else if (OperatingSystem.IsLinux())
            {
                var paths = Linux_GetDarksporeInstallPathFromProcess(process);
                exePath = paths.DarksporeExePath;
                winePrefix = paths.WINEPrefixPath;
                wineExecutable = paths.WINEExecutablePath;
            }

            
            if (exePath == null)
                return false;
            string exeName = Path.GetFileNameWithoutExtension(exePath);

            bool containsDarkspore = exeName.Contains("darkspore", StringComparison.OrdinalIgnoreCase);
            bool notServer = !exeName.Contains("server", StringComparison.OrdinalIgnoreCase);
            bool isExe = Path.GetExtension(exePath).Equals(".exe", StringComparison.OrdinalIgnoreCase);
            if (containsDarkspore)
                Debug.WriteLine($"'{exePath}':\n\t{containsDarkspore}\n\t{notServer}\n\t{isExe}");

            if (containsDarkspore
            && notServer
            && isExe
            )
            {
                bool showMsgBoxes = false;
                /*if (_showMsgBoxes)
                {
                    showMsgBoxes = true;
                    _showMsgBoxes = false;
                }
                string installPath = exePath;
                while (!Directory.Exists(Path.Combine(installPath, "DarksporeBin")))
                {
                    try
                    {
                        string pathParent = Path.GetDirectoryName(installPath);
                        if (string.IsNullOrEmpty(pathParent) || string.IsNullOrWhiteSpace(pathParent) || (pathParent.Equals(installPath, StringComparison.OrdinalIgnoreCase)))
                        {
                            break;
                        }

                        installPath = pathParent;
                        
                        if (OperatingSystem.IsWindows() && showMsgBoxes)
                            WindowsPInvoke.MessageBox(IntPtr.Zero, $"installPath: '{installPath}'", string.Empty, 0);
                    }
                    catch
                    {
                        installPath = null;
                        break;
                    }
                }
                //(!Path.GetFileName(installPath).Equals("DarksporeBin", StringComparison.OrdinalIgnoreCase));

                if (OperatingSystem.IsWindows() && showMsgBoxes)
                        WindowsPInvoke.MessageBox(IntPtr.Zero, $"installPath: '{installPath}'", "FINAL", 0);
                
                if (showMsgBoxes)
                    _showMsgBoxes = true;

                if (installPath == null)
                    return false;
                darksporeInstallPath = installPath;
                */
                string installPath1 = Path.GetDirectoryName(exePath);
                string installPath = Path.GetDirectoryName(Path.GetDirectoryName(exePath));
                /*if (OperatingSystem.IsWindows() && (!installPath.EndsWith('\\')))
                    installPath = installPath + "\\";*/
                
                /*if (Path.GetFileName(installPath).Equals("DarksporeBin", StringComparison.OrdinalIgnoreCase))
                    installPath =  Path.GetDirectoryName(installPath);
                if (OperatingSystem.IsWindows())
                    installPath = installPath.TrimEnd('\\');*/

                /*if (OperatingSystem.IsWindows() && (!installPath.EndsWith('\\')))
                    installPath = installPath + "\\";*/
                
                darksporeInstallPath = installPath;
                process.Kill();

                Debug.WriteLine($"DARKSPORE INSTALL PATH: '{darksporeInstallPath}'");
                return darksporeInstallPath != null;
            }
            return false;
        }

        [SupportedOSPlatform("windows")]
        string Windows_GetDarksporeInstallPathFromProcess(Process process)
        {
            return process.GetExecutablePath();
        }

        
        [SupportedOSPlatform("linux")]
        (string WINEPrefixPath, string WINEExecutablePath, string DarksporeExePath) Linux_GetDarksporeInstallPathFromProcess(Process process)
        {
            if (TryGetWINEInfo(process, out string winePrefix, out string wineExecutable, out string programExecutable, out int wineEsync))
            {
                return (winePrefix, wineExecutable, programExecutable);
            }
            return (null, null, null);
        }

        /// <summary>
        /// Takes a path to a symbolic link and returns the link target path.
        /// </summary>
        /// <param name="path">The path to the symlink</param>
        /// <returns>
        /// Returns the link to the target path on success; and null otherwise.
        /// </returns>
        [SupportedOSPlatform("linux")]
        static string? ReadLink(string path)
        {
            int bufferSize = 256;
            while (true)
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
                try
                {
                    int resultLength = LinuxPInvoke.ReadLink(path, buffer, buffer.Length);
                    if (resultLength < 0)
                    {
                        // error
                        return null;
                    }
                    else if (resultLength < buffer.Length)
                    {
                        // success
                        return Encoding.UTF8.GetString(buffer, 0, resultLength);
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }

                // buffer was too small, loop around again and try with a larger buffer.
                bufferSize *= 2;
            }
        }

        [SupportedOSPlatform("linux")]
        const string WINE_EXEC = "_=";
        [SupportedOSPlatform("linux")]
        const string PREFIX = "WINEPREFIX=";
        [SupportedOSPlatform("linux")]
        const string ESYNC = "WINEESYNC=";

        const string EXE_EXT = ".exe";
        static readonly char[] SMAPS_SPLIT = { '\0', '\r', '\n' };
        //wineEsync
        [SupportedOSPlatform("linux")]
        static bool TryGetWINEInfo(Process process, out string winePrefix, out string wineExecutable, out string programExecutable, out int wineEsync)
        {
            programExecutable = null;
            winePrefix = null;
            wineExecutable = null;
            wineEsync = -129;
            if (process == null)
                return false;
            else if (process.HasExited)
                return false;

            //https://stackoverflow.com/questions/1585989/how-to-parse-proc-pid-cmdline
            //`cat -v /proc/[PID]/cmdline`
            //`cat /proc/[PID]/cmdline | strings -1`
            //`cat /proc/[PID]/cmdline | strings -n 1`
            /*var cmdline = GetOutputForSProc(process.Id, "cmdline").Split('\0');
            foreach (string line in cmdline)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Contains(EXE_EXT, StringComparison.OrdinalIgnoreCase))
                {
                    string arg = line.Trim().Trim('\'', '"').Trim();
                    programExecutable = arg.Substring(0, arg.IndexOf(EXE_EXT) + EXE_EXT.Length);
                    break;
                }
            }
            if (programExecutable == null)
                return false;*/
            
            var environ = GetOutputForSProc(process.Id, "environ");
            if (environ.Trim().EndsWith("Permission denied"))
                return false;
            var environLines = environ.Split('\0');
            //File.WriteAllLines(@"/home/splitwirez/Documents/Spore Modding/Proton/steam-spore-env-vars.txt", lines);
            foreach (string line in environLines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    continue;
                
                if (line.StartsWith(PREFIX) && (winePrefix == null))
                    winePrefix = line.Substring(PREFIX.Length);
                else if (line.StartsWith(WINE_EXEC) && (wineExecutable == null))
                    wineExecutable = line.Substring(WINE_EXEC.Length);
                else if (line.StartsWith(ESYNC) && (wineEsync == -129) && int.TryParse(line.Substring(ESYNC.Length), out int esync))
                    wineEsync = esync;
            }
            if ((winePrefix == null) || (wineExecutable == null))
                return false;
            
            var smaps = GetOutputForSmaps(process.Id).Split(SMAPS_SPLIT, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (string line in smaps)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                    continue;
                
                if (line.Contains(EXE_EXT, StringComparison.OrdinalIgnoreCase) && (programExecutable == null))
                {
                    string exePath = line.Substring(0, line.LastIndexOf(EXE_EXT, StringComparison.OrdinalIgnoreCase) + EXE_EXT.Length);
                    Debug.WriteLine($"exePath: '{exePath}'");
                    if ((exePath != null) && exePath.Contains('/'))
                    {
                        exePath = exePath.Substring(exePath.IndexOf('/')).Trim().Trim('\'', '"').Trim();
                        
                        if (File.Exists(exePath) && Path.GetExtension(exePath).Equals(EXE_EXT, StringComparison.OrdinalIgnoreCase))
                        {
                            programExecutable = exePath;
                            Debug.WriteLine("GOOD EXE PATH");
                        }
                    }
                }
                /*else if ((winePrefix != null) && (wineExecutable != null))
                    break;*/
            }
            var sproc = SProcFor(process.Id, "exe");
            if (sproc == null)
                return false;
            var link = ReadLink(sproc);
            if (link == null)
                return false;
            var dir = Path.GetDirectoryName(link);
            if (dir == null)
                return false;
            wineExecutable = Path.Combine(dir, "wine");
            //Console.WriteLine($"[wineExecutable dir: {Path.GetDirectoryName(wineExecutable)}]");
            //Console.WriteLine($"[wineExecutable: \"{wineExecutable}\"]");
            //Console.WriteLine($"[winePrefix \"{winePrefix}\"]");
            //lines = GetOutputForSmaps(process.Id).Split('\0');
            /*if ((winePrefix != null) && (wineExecutable != null))
            {
                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = wineExecutable,
                    Arguments = $"winepath -u '{programExecutable}'"
                };
                processStartInfo.Environment.Add("WINEPREFIX", winePrefix);
                programExecutable = GetOutputForCommand(processStartInfo);
                return (programExecutable != null) && File.Exists(programExecutable);
            }
            return false;*/
            return (winePrefix != null) && (wineExecutable != null) && (programExecutable != null);
        }


        
        [SupportedOSPlatform("linux")]
        static string GetOutputForSmaps(int pid)
            => GetOutputForSProc(pid, "smaps");
        [SupportedOSPlatform("linux")]
        static string GetOutputForCommand(ProcessStartInfo startInfo)
        {
            var procStartInfo = startInfo;
            procStartInfo.UseShellExecute = false;
            procStartInfo.RedirectStandardInput = true;
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            var get = new Process()
            {
                StartInfo = procStartInfo
            };
            //get.StandardOutput
            get.Start();
            string output = get.StandardOutput.ReadToEnd();
            get.WaitForExit();
            return output;
        }
        [SupportedOSPlatform("linux")]
        static string GetOutputForCommand(string exec, string args)
            => GetOutputForCommand(new ProcessStartInfo()
                {
                    FileName = exec,
                    Arguments = args
                });
        [SupportedOSPlatform("linux")]
        static string GetOutputForSProc(int pid, string subCmd)
            => GetOutputForCommand(GetProcCmd(pid, subCmd));
        [SupportedOSPlatform("linux")]
        static ProcessStartInfo GetProcCmd(int pid, string subCmd)
            => new ProcessStartInfo()
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                //https://serverfault.com/questions/66363/environment-variables-of-a-running-process-on-unix/66366#66366
                FileName = "cat",
                Arguments = SProcFor(pid, subCmd)
            };
        [SupportedOSPlatform("linux")]
        static string SProcFor(int pid, string subCmd)
            => $"/proc/{pid}/{subCmd}";

        void TrySetResult(string result)
        {
            _tcs.TrySetResult(result);
            
            _darksporeProcessTimer?.Stop();
        }

        
        public void Cancel(object parameter = null)
            => TrySetResult(null);

        public void Accept(object parameter = null)
            => TrySetResult(GamePath);
    }
}
