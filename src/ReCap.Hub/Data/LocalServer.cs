using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    public class LocalServer : ViewModelBase
    {
        const string DBPF_RES_PREFIX = "dbpf?";
        const string SRV_RES_PREFIX = "recap-server?";
        
        public static readonly string AUTO_LOGIN_PACKAGE_RESNAME = $"{DBPF_RES_PREFIX}{Patcher.AUTO_LOGIN_PACKAGE_NAME}";
        public static readonly LocalServer Instance = null;

        /*string _serverDirPath = string.Empty;
        string _serverExePath = string.Empty;*/
        
        
        static LocalServer()
        {
            Instance = new LocalServer();
            //InstanceCreated?.Invoke(Instance, new EventArgs());
        }
        //public static event EventHandler InstanceCreated;
        private LocalServer()
        {
            //_serverDirPath = Path.Combine(HubData.Instance.CfgPath, "Server");
            string serverDir = HubGlobalPaths.ServerDir;
            if (!Directory.Exists(serverDir))
                Directory.CreateDirectory(serverDir);

            foreach (string resName in GetResourceNames())
            {
                if (!resName.StartsWith(SRV_RES_PREFIX))
                    continue;

                string resFileName = resName.Substring(SRV_RES_PREFIX.Length);
                string destinationPath = Path.Combine(serverDir, resFileName);
                
                if (Path.GetFileName(resFileName).Equals("placekeeper", StringComparison.OrdinalIgnoreCase))
                {
                    string dir = Path.GetDirectoryName(destinationPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    
                    continue;
                }

                ExtractResource(resName, destinationPath);
            }
        }

        static IEnumerable<string> GetResourceNames()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            return a.GetManifestResourceNames();
        }

        public static void ExtractResource(string resourceName, string destinationPath)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            string dir = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream resStream = a.GetManifestResourceStream(resourceName))
            {
                if (File.Exists(destinationPath))
                    File.Delete(destinationPath);
                using (var fileStream = File.Open(destinationPath, FileMode.Create))
                {
                    resStream.CopyTo(fileStream);
                }
            }
        }


        /*public async Task<bool> RunPatcher(string exeSrcPath, string exeDestPath, string dataDirPath)
        {
            string patcherDir = Path.Combine(_serverDirPath, "patcher");
            string args = string.Empty;
            if ((exeSrcPath != null) && (exeDestPath != null))
            {
                args += CommandLine.PrepareArgForCLI(exeSrcPath) + " " + CommandLine.PrepareArgForCLI(exeDestPath);
            }

            if (dataDirPath != null)
            {
                if (string.IsNullOrEmpty(args) || string.IsNullOrWhiteSpace(args))
                {
                    args = "--copy-package ";
                }
                else
                    args += " ";
                args += CommandLine.PrepareArgForCLI(Path.Combine(patcherDir, AUTO_LOGIN_PACKAGE_NAME)) + " " + CommandLine.PrepareArgForCLI(Path.Combine(dataDirPath, AUTO_LOGIN_PACKAGE_NAME));
            }


            try
            {
                string patcherPath = Path.Combine(patcherDir, "patch_darkspore_exe.exe");
                int exitCode = -1;
                //https://7thzero.com/blog/process-startinfo-redirectstandardoutput-not-quite-what-you-d-ex
                Process patcherProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo(patcherPath)
                    {
                        Verb = "runas",
                        //WorkingDirectory = darksporeBin,
                        Arguments = args,
                        UseShellExecute = true,
                    }
                };


                await Task.Run(() =>
                {
                    patcherProcess.Start();
                    patcherProcess.WaitForExit();
                    exitCode = patcherProcess.ExitCode;
                    //Debug.WriteLine($"patcherProcess exit code: {patcherProcess.ExitCode}");
                });

                return exitCode == 0;
            }
            catch
            {
                return false;
            }
        }*/

        public ActionableDisposable Start(string winePrefix, string wineExecutable)
        {
            bool useWine = !OperatingSystem.IsWindows();
            string serverDir = HubGlobalPaths.ServerDir;
            string serverExeName = useWine
                ? "recap_server-wine.exe" //TODO: swap dumb temporary executable that attaches to the wrong port so it can start at all under WINE for a native server executable
                : "recap_server.exe"
            ;
            string serverExePath = Path.Combine(serverDir, serverExeName);
            Process serverProcess = WineHelper.PrepareRunUnderWINE(winePrefix, wineExecutable, serverExePath, serverDir, null, true);

            /*serverProcess.StartInfo.CreateNoWindow = true;
            serverProcess.StartInfo.UseShellExecute = false;*/
            //serverProcess.StartInfo.RedirectStandardInput = true;
            serverProcess.StartInfo.RedirectStandardOutput = true;
            serverProcess.StartInfo.RedirectStandardError = true;
            if (useWine)
                serverProcess.StartInfo.CreateNoWindow = true;
            SynchronizationContext ctx = SynchronizationContext.Current;


            //serverProcess.Exited += (s, e) => ServerExited?.Invoke(this, new EventArgs());
            serverProcess.OutputDataReceived += (s, e) => Debug.WriteLine("==== SERVER LOG\n\t" + e.Data);
            serverProcess.ErrorDataReceived += (s, e) => Debug.WriteLine("==== SERVER ERROR\n\t" + e.Data);
            var thread = new Thread(() =>
            {
                serverProcess.WaitForExit();
                //ctx.Post(_ => {
                    ServerExited?.Invoke(this, new EventArgs());
                //}, null);
            });
            try
            {
                serverProcess.Start();
                thread.Start();
                ServerStarted?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Debug.Write(ex.StackTrace);
            }

            return new ActionableDisposable(() =>
            {
                if (useWine)
                    Process.Start("killall", serverExeName)?.WaitForExit();
                else
                    serverProcess.Kill(true);
            });
        }

        public static event EventHandler ServerStarted;
        public static event EventHandler ServerExited;
    }
}