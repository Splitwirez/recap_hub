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
    public partial class LocateDarksporeViewModel
        : ViewModelBase
        , IDialogViewModel<DarksporeInstallPaths>
    {   
        [SupportedOSPlatform("linux")]
        DarksporeInstallPaths Linux_GetDarksporeInstallPathFromProcess(Process process)
        {
            if (TryGetWINEInfo(process, out string winePrefix, out string wineExecutable, out string programExecutable, out int wineEsync))
                return new DarksporeInstallPaths(winePrefix, wineExecutable, programExecutable);
            else
                return new DarksporeInstallPaths();
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
                    if ((exePath != null) && exePath.Contains('/'))
                    {
                        exePath = exePath.Substring(exePath.IndexOf('/')).Trim().Trim('\'', '"').Trim();
                        //Debug.WriteLine($"exePath: '{exePath}'");
                        
                        if (File.Exists(exePath) && Path.GetExtension(exePath).Equals(EXE_EXT, StringComparison.OrdinalIgnoreCase))
                        {
                            programExecutable = exePath;
                            //Debug.WriteLine("GOOD EXE PATH");
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

        
        [SupportedOSPlatform("linux")]
        static bool Linux_TryGetDefaultWINEExecutable(out string wineExec)
        {
            string wine = "wine";
            wineExec = null;
            try
            {
                wineExec = 
                    //Path.GetFullPath(wine)
                    GetOutputForCommand(new ProcessStartInfo("which", wine))
                ;
                
                if (wineExec != null)
                    wineExec = wineExec.Trim().Trim('\'', '"').Trim();
                
                Debug.WriteLine($"WINE executable 0: '{wineExec}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message}\n{ex.StackTrace}");

                try
                {
                    string whereisWineStart = $"{wine}:";
                    string whereisWine = GetOutputForCommand("whereis", wine);
                    if (whereisWine.StartsWith(whereisWineStart))
                    {
                        whereisWine = whereisWine.Substring(whereisWineStart.Length);
                    }

                    if (whereisWine.Contains(' '))
                        whereisWine = whereisWine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0];

                    wineExec = whereisWine.Trim().Trim('\'', '"').Trim();
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine($"ex2: {ex2.Message}\n{ex2.StackTrace}");
                }
            }

            if ((string.IsNullOrEmpty(wineExec) || string.IsNullOrWhiteSpace(wineExec)))
                wineExec = null;
            else
            {
                try
                {
                    Debug.WriteLine($"{nameof(Linux_TryGetDefaultWINEExecutable)} 1: {wineExec != null}");
                    if (File.Exists(wineExec))
                        return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ex: {ex.Message}\n{ex.StackTrace}");
                }
            }
            Debug.WriteLine($"{nameof(Linux_TryGetDefaultWINEExecutable)} 2: {wineExec != null}");
            wineExec = null;
            return false;
        }


        [SupportedOSPlatform("linux")]
        static bool Linux_TryGetDefaultWINEPrefix(out string winePrefix)
        {
            string envVar = "WINEPREFIX";
            try
            {
                string winePrefixPath = Environment.GetEnvironmentVariable(envVar);
                if (!(string.IsNullOrEmpty(winePrefixPath) || string.IsNullOrWhiteSpace(winePrefixPath)))
                {
                    if (Directory.Exists(winePrefixPath))
                    {
                        winePrefix = winePrefixPath;
                        Debug.WriteLine($"{nameof(Linux_TryGetDefaultWINEPrefix)} 1: {winePrefix != null}");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message}\n{ex.StackTrace}");
            }
            Debug.WriteLine($"{nameof(Linux_TryGetDefaultWINEPrefix)} 2: false");
            winePrefix = null;
            return false;
        }
    }
}
