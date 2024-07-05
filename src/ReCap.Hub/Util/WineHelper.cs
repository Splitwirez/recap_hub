using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReCap.Hub.Data;

namespace ReCap.Hub
{
    public static class WineHelper
    {
        const string CD_PATH_DEFAULT = null;
        const bool OR_WIN_NATIVE_DEFAULT = true;
        const string[] ARGS_DEFAULT = null;
        /*public static Process PrepareRunUnderWINE(string winePrefix, string wineExecutable, string command, string[] args = ARGS_DEFAULT)
            => PrepareRunUnderWINE(winePrefix, wineExecutable, command, CD_PATH_DEFAULT, OR_WIN_NATIVE_DEFAULT, args);*/

        public static Process PrepareRunUnderWINE(string winePrefix, string wineExecutable, string command, bool orNativeIfOnWindows)
            => PrepareRunUnderWINE(winePrefix, wineExecutable, command, CD_PATH_DEFAULT , ARGS_DEFAULT, orNativeIfOnWindows);
        public static Process PrepareRunUnderWINE(string winePrefix, string wineExecutable, string command, string cdPath, string[] args, bool orNativeIfOnWindows)
            => PrepareRunUnderWINECore(winePrefix, wineExecutable, command, cdPath, args, orNativeIfOnWindows);
        static Process PrepareRunUnderWINECore(string winePrefix, string wineExecutable, string command, string cdPath, string[] args, bool orNativeIfOnWindows)
        {
            if (orNativeIfOnWindows && OperatingSystem.IsWindows())
            {
                var startInfo = new ProcessStartInfo(command)
                {
                    UseShellExecute = false
                };
                if (cdPath != null)
                    startInfo.WorkingDirectory = cdPath;

                if (args != null)
                    startInfo.Arguments = CommandLine.PrepareArgsForCLI(args);


                return new Process()
                {
                    StartInfo = startInfo
                };
            }
            else
            {
                List<string> wineRawArgs = new()
                {
                    "start",
                    "/wait",
                    UnixPathToWINEPath(winePrefix, wineExecutable, command),
                };
                if (args != null)
                    wineRawArgs.AddRange(args);


                return PrepareRunUnderWINERaw(winePrefix, wineExecutable, cdPath, wineRawArgs.ToArray());
            }
        }
        
        static string[] PrepArgs(List<string> args, string[] moreArgs)
        {
            var ret = args; //.AsQueryable();
            if (moreArgs != null)
                ret.AddRange(moreArgs);
            
            return ret.ToArray();
        }

        
        static Process PrepareRunUnderWINERaw(string winePrefix, string wineExecutable, string[] args)
            => PrepareRunUnderWINERaw(winePrefix, wineExecutable, null, args);
        static Process PrepareRunUnderWINERaw(string winePrefix, string wineExecutable, string cdPath, string[] args)
        {
            var startInfo = new ProcessStartInfo(
                wineExecutable
                , CommandLine.PrepareArgsForCLI(args)
            )
            {
                UseShellExecute = false,
            };
            
            if (cdPath != null)
                startInfo.WorkingDirectory = cdPath;
            
            Debug.WriteLine($"{nameof(PrepareRunUnderWINE)} args:\n\t{startInfo.Arguments}");
            startInfo.EnvironmentVariables.Add("WINEPREFIX", winePrefix);
            
            /*startInfo.ArgumentList.Add(CommandLine.PrepareArgForCLI(command));
            if (args != null)
            {
                foreach (var arg in args)
                {
                    startInfo.ArgumentList.Add(CommandLine.PrepareArgForCLI(arg));
                }
            }*/

            return new Process()
            {
                StartInfo = startInfo
            };
        }

        public static string GetWINEEnvironmentVariable(string winePrefix, string wineExecutable, string variable)
        
            => GetWINECommandOutput(PrepareRunUnderWINERaw(winePrefix, wineExecutable, new[]
            {
                "cmd.exe",
                "/c",
                $"'echo %{variable}%'"
            }));

        public static string GetWINECommandOutput(Process commandProcess, bool trim = true)
        {
            commandProcess.StartInfo.UseShellExecute = false;
            commandProcess.StartInfo.RedirectStandardInput = true;
            commandProcess.StartInfo.RedirectStandardOutput = true;
            commandProcess.StartInfo.RedirectStandardError = true;

            commandProcess.Start();
			
			string outPath = commandProcess.StandardOutput.ReadToEnd();
			commandProcess.WaitForExit();
			
            if (trim)
			    outPath = outPath.Trim();
            
            return outPath;
        }

        public static string UnixPathToWINEPath(string winePrefix, string wineExecutable, string unixPath)
            => WinepathConvert(winePrefix, wineExecutable, unixPath, true);
        
        public static string WINEPathToUnixPath(string winePrefix, string wineExecutable, string unixPath)
            => WinepathConvert(winePrefix, wineExecutable, unixPath, false);

        static string WinepathConvert(string winePrefix, string wineExecutable, string inPath, bool toWINE)
            => GetWINECommandOutput(PrepareRunUnderWINERaw(winePrefix, wineExecutable, new string[]
            {
                "winepath",
                toWINE
                    ? "-w"
                    : "-u"
                , inPath
            }));
    }
}
