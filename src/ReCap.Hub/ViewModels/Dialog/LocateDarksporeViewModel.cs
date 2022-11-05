using Avalonia.Platform.Storage;
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
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public partial class LocateDarksporeViewModel : ViewModelBase, IDialogViewModel<(string winePrefix, string wineExecutable, string darksporeInstallPath)>
    {
        const string EXE_EXT = ".exe";


        TaskCompletionSource<(string winePrefix, string wineExecutable, string darksporeInstallPath)> _tcs = new TaskCompletionSource<(string winePrefix, string wineExecutable, string darksporeInstallPath)>();
        public TaskCompletionSource<(string winePrefix, string wineExecutable, string darksporeInstallPath)> GetCompletionSource()
        {
            return _tcs;
        }

        string _winePrefix = null;
        string _wineExecutable = null;
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

        string _gamePathFromBrowseDialog = string.Empty;
        public string GamePathFromBrowseDialog
        {
            get => _gamePathFromBrowseDialog;
            set => RASIC(ref _gamePathFromBrowseDialog, value);
        }

        static readonly FilePickerOpenOptions DARKSPORE_EXE_PICKER = new()
        {
            FileTypeFilter = new List<FilePickerFileType>()
            {
                new("Darkspore game executable")
                {
                    Patterns = new List<string>()
                    {
                        "Darkspore*.exe"
                    }.AsReadOnly(),
                    AppleUniformTypeIdentifiers = new List<string>()
                    {
                        "public.windows-executable"
                    }.AsReadOnly(),
                }
            }
        };


        public async void BrowseForGameExeCommand(object parameter)
        {
            string exePath = await BrowseForGameExe();
            if (exePath != null)
                GamePathFromBrowseDialog = exePath;
        }

        public async Task<string> BrowseForGameExe()
        {
            return await BrowseDialogService.OpenFileAsync(DARKSPORE_EXE_PICKER);
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

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(BrowseForGamePath))
                {

                }
            };
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
                    _wineExecutable = wineExecutable;
                    _winePrefix = winePrefix;
                    GamePath = darksporeInstallPath;
                    Accept();
                }
            }
        }

        //static bool _showMsgBoxes = true;
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
            
            if (IsProcessDarkspore(exePath))
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

                //Debug.WriteLine($"DARKSPORE INSTALL PATH: '{darksporeInstallPath}'");
                return darksporeInstallPath != null;
            }
            return false;
        }

        public static bool IsProcessDarkspore(string exePath)
        {
            string exeName = Path.GetFileNameWithoutExtension(exePath);

            bool containsDarkspore = exeName.Contains("darkspore", StringComparison.OrdinalIgnoreCase);
            bool notServer = !exeName.Contains("server", StringComparison.OrdinalIgnoreCase);
            bool isExe = Path.GetExtension(exePath).Equals(".exe", StringComparison.OrdinalIgnoreCase);
            /*if (containsDarkspore)
            {
                Debug.WriteLine($"'{exePath}':\n\t{containsDarkspore}\n\t{notServer}\n\t{isExe}");
            }*/

            return containsDarkspore
                && notServer
                && isExe
            ;
        }

        void TrySetResult(string winePrefix, string wineExecutable, string darksporeInstallPath)
        {
            _tcs.TrySetResult((winePrefix, wineExecutable, darksporeInstallPath));
            
            _darksporeProcessTimer?.Stop();
        }

        
        public void Cancel(object parameter = null)
            => TrySetResult(null, null, null);

        public void Accept(object parameter = null)
            => TrySetResult(_winePrefix, _wineExecutable, GamePath);
    }
}
