using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public class GameConfigViewModel : ViewModelBase
    {
        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        double _lastLaunchTime = -1;
        public double LastLaunchTime
        {
            get => _lastLaunchTime;
        }


        ObservableCollection<SaveGameViewModel> _saves = new ObservableCollection<SaveGameViewModel>();
        public ObservableCollection<SaveGameViewModel> Saves
        {
            get => _saves;
            set
            {
                if (_saves != null)
                    _saves.CollectionChanged -= Saves_CollectionChanged;
                RASIC(ref _saves, value);

                if (value != null)
                    value.CollectionChanged += Saves_CollectionChanged;
            }
        }

        protected void Saves_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (SaveGameViewModel save in e.NewItems)
                {
                    //save.EnableFSWatcher = true;
                }
            }

            if (e.OldItems != null)
            {
                foreach (SaveGameViewModel save in e.OldItems)
                {
                    //save.EnableFSWatcher = false;
                }
            }
        }


        SaveGameViewModel _selectedSave = null;
        public SaveGameViewModel SelectedSave
        {
            get => _selectedSave;
            set => RASIC(ref _selectedSave, value);
        }


        string _gameInstallPath = string.Empty;
        public string GameInstallPath
        {
            get => _gameInstallPath;
            set => RASIC(ref _gameInstallPath, value);
        }

        string _savesPath = string.Empty;
        public string SavesPath
        {
            get => _savesPath;
            set => RASIC(ref _savesPath, value);
        }


        string _winePrefixPath = null;
        public string WinePrefixPath
        {
            get => _winePrefixPath;
            set => RASIC(ref _winePrefixPath, value);
        }


        string _wineExecPath = null;
        public string WineExecPath
        {
            get => _wineExecPath;
            set => RASIC(ref _wineExecPath, value);
        }

        public GameConfigViewModel(string gameInstallPath, string wineExecutable, string winePrefix) //, string savesPath)
            : this()
        {
            GameInstallPath = gameInstallPath;
            WinePrefixPath = winePrefix;
            WineExecPath = wineExecutable;
            SavesPath = HubGlobalPaths.ServerAccountsDir; //savesPath;
        }

        public GameConfigViewModel(string gameInstallPath, string wineExecutable, string winePrefix, ref XElement element) //, string savesPath)
            : this(gameInstallPath, wineExecutable, winePrefix)
        {
            if (Directory.Exists(SavesPath))
            {
                string savesPath = SavesPath;
                List<string> filePaths = Directory.EnumerateFiles(SavesPath)
                    //.Where(x => !Path.GetFileName(x).Equals("placekeeper", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (var el in element.Elements())
                {
                    if (!el.TryGetAttributeValue("id", out string saveFileName))
                        continue;
                    string saveXmlPath = Path.Combine(savesPath, saveFileName + ".xml");
                    if (!File.Exists(saveXmlPath))
                        continue;
                    
                    Saves.Add(new SaveGameViewModel(saveXmlPath));
                }
                /*foreach (string f in filePaths)
                {
                    if (Path.GetFileName(f).Equals("placekeeper", StringComparison.OrdinalIgnoreCase))
                    {
                        File.Delete(f);
                        continue;
                    }

                    try
                    {
                        var save = new SaveGameViewModel(f);
                        Saves.Add(save);
                    }
                    catch (XmlException ex)
                    { }
                }*/
            }
        


            if (TimeHelper.TryGetNewest(Saves, s => s.LastLaunchTime, out SaveGameViewModel lastPlayed))
                SelectedSave = lastPlayed;
        }
            
        private GameConfigViewModel()
        : base()
        {
            /*LocalServer.InstanceCreated += LocalServer_InstanceCreated;
            try
            {
                
            }
            catch (NullReferenceException ex)
            {
                
            }*/
            EnsureServerExitedHandler(LocalServer.Instance);
        }

        /*private void LocalServer_InstanceCreated(object sender, EventArgs e)
        {
            
            LocalServer.InstanceCreated -= LocalServer_InstanceCreated;
        }*/
        void EnsureServerExitedHandler(LocalServer server)
        {
            LocalServer.ServerExited += (s, e) =>
            {
                /*foreach (var save in Saves)
                {
                    save.ReadFromXml();
                }*/
            };
        }

        public async void NewSaveGameCommand(object parameter)
        {
            var task = CreateSaveGame(true);
            await task;
            if (task.IsFaulted || (task.Exception != null))
                throw task.Exception;
        }

        public async void DeleteSaveGameCommand(object parameter)
        {
            if (!(parameter is SaveGameViewModel deletThis))
                return;
                
            var task = DeleteSaveGame(deletThis);
            await task;
            if (task.IsFaulted || (task.Exception != null))
                throw task.Exception;
        }
        public async Task DeleteSaveGame(SaveGameViewModel saveGame)
        {
            if (!Saves.Contains(saveGame))
                return;
            
            if (await DialogDisplay.ShowDialog(new YesNoDialogViewModel("Delete save game", "Are you sure you want to DELETE this save game?")))
            {
                Saves.Remove(saveGame);
                saveGame.Delete();
                HubData.Instance.Save();
            }
        }

        public async void RenameSaveGameCommand(object parameter)
        {
            Debug.WriteLine(nameof(RenameSaveGameCommand));
            Console.WriteLine(nameof(RenameSaveGameCommand));
            if (!(parameter is SaveGameViewModel ren))
                return;
            
            
            var task = RenameSaveGame(ren);
            await task;
            if (task.IsFaulted || (task.Exception != null))
                throw task.Exception;
        }
        public async Task RenameSaveGame(SaveGameViewModel saveGame)
        {
            if (!Saves.Contains(saveGame))
                return;
            
            string oldTitle = saveGame.Title;
            string newTitle = await DialogDisplay.ShowDialog(new TextBoxDialogViewModel("Rename save game", string.Empty, oldTitle, true));
            if ((newTitle != null) && (newTitle != oldTitle) && (!(string.IsNullOrEmpty(newTitle) || string.IsNullOrWhiteSpace(newTitle))))
            {
                saveGame.Rename(newTitle);
                HubData.Instance.Save();
            }
        }

        public async Task<SaveGameViewModel> CreateSaveGame(bool isCloseable = true)
        {
            var saveGame = await DialogDisplay.ShowDialog(new NewSaveGameViewModel(SavesPath, isCloseable));
            if (saveGame != null)
            {
                Saves.Add(saveGame);
                SelectedSave = saveGame;
                HubData.Instance.Save();
            }
            return saveGame;
        }

        public async void PlayGameWithSaveCommand(object parameter)
        {
            if (!(parameter is SaveGameViewModel save))
                return;
                
            var task = PlayGameWithSave(save);
            await task;
            if (task.IsFaulted || (task.Exception != null))
                throw task.Exception;
        }

        static IDisposable _lastServerInstance = null;
        public async Task PlayGameWithSave(SaveGameViewModel save)
        {
            double now = DateTime.UtcNow.ToUniversalTime().Subtract(DateTime.UnixEpoch.ToUniversalTime()).TotalMilliseconds;
            
            
            string installPath = GameInstallPath;
            bool installPathOK = IsPathOK(installPath);
            Debug.WriteLine($"installPath: '{installPath}' {installPathOK}");

            bool useWine = !OperatingSystem.IsWindows();
            bool wineExecPathOK = useWine ? IsPathOK(WineExecPath, false) : true;
            Debug.WriteLine($"wineExecPath: '{WineExecPath}' {wineExecPathOK}");
            bool winePrefixPathOK = useWine ? IsPathOK(WinePrefixPath) : true;
            Debug.WriteLine($"winePrefixPath: '{WinePrefixPath}' {winePrefixPathOK}");
            if (!(
                installPathOK
                && wineExecPathOK
                && winePrefixPathOK
            ))
            {
                var task = DialogDisplay.ShowDialog(new LocateDarksporeViewModel());
                var paths = await task;
                if (task.IsFaulted || (task.Exception != null))
                    throw task.Exception;
                
                if (!installPathOK)
                {
                    installPath = paths.DarksporeInstallPath;
                    GameInstallPath = installPath;
                }
                
                if (!wineExecPathOK)
                    WineExecPath = paths.WineExecutable;
                
                if (!winePrefixPathOK)
                    WinePrefixPath = paths.WinePrefix;
                
                HubData.Instance.Save();
            }

            string appDataDir = useWine
                ? WineHelper.GetWINEEnvironmentVariable(WinePrefixPath, WineExecPath, "appdata")
                : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            ;
            string prefsDirPath = Path.Combine(appDataDir, "DarksporeData", "Preferences");

            if (!Directory.Exists(prefsDirPath))
                Directory.CreateDirectory(prefsDirPath);
            
            string loginPropPath = Path.Combine(prefsDirPath, "login.prop");
            string loginPropText = $"UserName \"{save.EmailAddress}\"\n";
            Debug.WriteLine($"Launching Darkspore for save: '{save.Title}'");
            File.WriteAllText(loginPropPath, loginPropText);
            //Debug.WriteLine($"path: '{loginPropPath}'\n\ntext:\n{loginPropText}");

            string gameBinPath = Path.Combine(GameInstallPath, "DarksporeBin");
            string patchedExeName = "Darkspore_ReCapPatched.exe";
            string gameExePath = Path.Combine(gameBinPath, patchedExeName);

            string gameDataPath = Path.Combine(GameInstallPath, "Data");
            string autoLoginPackageDestPath = Path.Combine(gameDataPath, Patcher.AUTO_LOGIN_PACKAGE_NAME);
            bool exeMissing = !File.Exists(gameExePath);
            bool autoLoginPackageMissing = !File.Exists(autoLoginPackageDestPath);

            string gameOriginalExePath = Path.Combine(gameBinPath, "Darkspore.exe");
            if (exeMissing || autoLoginPackageMissing)
            {
                Patcher.PatchGame(exeMissing, gameOriginalExePath, gameExePath, autoLoginPackageMissing, autoLoginPackageDestPath);

                /*await LocalServer.Instance.RunPatcher(
                    exeMissing ? gameOriginalExePath : null
                    , exeMissing ? gameExePath : null
                    , autoLoginPackageMissing ? gameDataPath : null
                );*/
            }


            //Process.GetCurrentProcess().Kill();
            save.UpdateUserDisplayName(HubData.Instance.UserDisplayName);

            _lastServerInstance?.Dispose();
            _lastServerInstance = LocalServer.Instance.Start(WinePrefixPath, WineExecPath);
            
            {
                var fail = await GameLaunchService.LaunchGame(WinePrefixPath, WineExecPath, gameExePath, gameBinPath);
                if (fail != null)
                {
                    //TODO: Do something useful here
                }

                /*Process[] darksporeProcesses = new Process[]
                {
                    darksporeProcess
                };
                bool started = false;*/
                /*await Task.Run(() =>
                {* /
                    /*do
                    {
                        if (!started)
                        {
                            darksporeProcess.Start();
                            Debug.WriteLine($"DARKSPORE ORIGINAL PROCESS NAME: {darksporeProcess.ProcessName}");
                            started = true;
                        }
                        while (!darksporeProcesses.All(x => x.HasExited))
                        //darksporeProcess.WaitForExit();
                        /*if (darksporeExit.IsFaulted || (darksporeExit.Exception != null))
                            throw darksporeExit.Exception;
                        else* /
                        {
                            darksporeProcesses = Process.GetProcesses().Where(x =>
                            {
                                try
                                {
                                    return LocateDarksporeViewModel.IsProcessDarkspore(x.GetExecutablePath());
                                }
                                catch
                                {
                                    return false;
                                }
                            }).ToArray(); //.GetProcessesByName(patchedExeName);
                            if (darksporeProcesses.Length <= 0)
                            {
                                Debug.WriteLine("DARKSPORE EXITED");
                                //darksporeExit = null;
                                darksporeProcesses = null;
                                break;
                            }
                            else
                            {
                                //darksporeExit = Task.WhenAll(darksporeProcesses.ToList().ConvertAll<Task>(x => x.WaitForExitAsync()).ToArray());
                            }
                        }
                    }
                    while ((darksporeProcesses != null) && (darksporeProcesses.Length > 0)); //((darksporeExit == null) || (!darksporeExit.IsCompleted));
                //});
                */

            }
            if (HubData.Instance.AutoCloseServer)
            {
                _lastServerInstance?.Dispose();
                _lastServerInstance = null;
            }
            Process.GetCurrentProcess().Kill();
            save.ReadFromXml(true);
            HubData.Instance.GameConfigs.Remove(this);
            HubData.Instance.GameConfigs.Insert(0, this);
            //TODO: HubData.Instance.SelectedGameConfig = this;
            _lastLaunchTime = now;
            save.LastLaunchTime = now;
            //save.ReadFromXml()



            Saves.Remove(save);
            Saves.Insert(0, save);
            SelectedSave = save;
            HubData.Instance.Save();
        }

        static bool IsPathOK(string path, bool isDirectory = true)
        {
            return
                (!string.IsNullOrEmpty(path))
                && (!string.IsNullOrWhiteSpace(path))
                && (
                    isDirectory
                        ? Directory.Exists(path)
                        : File.Exists(path)
                )
            ;
        }

        public void WriteToXml(ref XElement gameConfigEl)
        {
            gameConfigEl.RemoveNodes();

            gameConfigEl.SetAttributeValue(HubData.WINE_PFX_ATTR, WinePrefixPath);
            gameConfigEl.SetAttributeValue(HubData.WINE_EX_ATTR, WineExecPath);
            gameConfigEl.SetAttributeValue(HubData.GAME_PATH_ATTR, GameInstallPath);
            gameConfigEl.SetAttributeValue(HubData.SAVES_PATH_ATTR, SavesPath);

            var newSaves = Saves.OrderBy(x => x.LastLaunchTime);
            foreach (var save in newSaves)
            {
                var saveEl = new XElement("save");
                save.WriteToXml(ref saveEl);
                gameConfigEl.Add(saveEl);
            }
        }
    }
}
