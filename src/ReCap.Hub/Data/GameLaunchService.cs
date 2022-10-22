using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.Data
{
    public static class GameLaunchService
    {
        public static async Task<Exception> LaunchGame(string winePrefix, string wineExecutable, string gameExecutable, string gameBinDir, params string[] commandLineOptions)
        {
            bool useWine = (!OperatingSystem.IsWindows());
            IEnumerable<string> cliOptions = commandLineOptions;
            if (useWine)
                cliOptions = cliOptions.Prepend("-w").Prepend("-nolauncher");

            
            //string launchArgs = CommandLine.PrepareArgsForCLI(cliOptions);
            Process darksporeProcess =
                WineHelper.PrepareRunUnderWINE(winePrefix, wineExecutable, gameExecutable, gameBinDir, 
                useWine
                    ? new[] { "-w", "-nolauncher" }
                    : new string[0]
                , true);


            //darksporeProcess.WaitForExit();
            try
            {
                darksporeProcess.Start();
                await darksporeProcess.WaitForExitAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted || (t.Exception != null))
                    {
                        throw t.Exception;
                    }
                });
                Debug.WriteLine("DARKSPORE EXITED");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DARKSPORE LAUNCH FAILED:\n\n"
                    + "\tException of type '{ex.GetType().FullName}' was thrown: '{ex.Message}'\n"
                    + ex.StackTrace);
                return ex;
            }
        }
    }
}
