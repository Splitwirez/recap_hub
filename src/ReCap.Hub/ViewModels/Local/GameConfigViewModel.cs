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
        public GameConfigViewModel(string gameInstallPath) //, string savesPath)
            : this()
        {
            GameInstallPath = gameInstallPath;
            SavesPath = HubGlobalPaths.ServerAccountsDir; //savesPath;
            if (Directory.Exists(SavesPath))
            {
                List<string> filePaths = Directory.EnumerateFiles(SavesPath)
                    //.Where(x => !Path.GetFileName(x).Equals("placekeeper", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                foreach (string f in filePaths)
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
                }
            }


            if (Saves.Count > 0)
                SelectedSave = Saves[0];
        }
            
        public GameConfigViewModel()
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
                foreach (var save in Saves)
                {
                    save.ReadFromXml();
                }
            };
        }

        public async void NewSaveGameCommand(object parameter)
        {
            await CreateSaveGame(true);
        }

        public async void DeleteSaveGameCommand(object parameter)
        {
            if (parameter is SaveGameViewModel deletThis)
                await DeleteSaveGame(deletThis);
        }
        public async Task DeleteSaveGame(SaveGameViewModel saveGame)
        {
            if (Saves.Contains(saveGame))
            {
                if (await DialogDisplay.ShowDialog(new YesNoDialogViewModel("Delete save game", "Are you sure you want to DELETE this save game?")))
                {
                    Saves.Remove(saveGame);
                    saveGame.Delete();
                }
            }
        }

        public async void RenameSaveGameCommand(object parameter)
        {
            Debug.WriteLine(nameof(RenameSaveGameCommand));
            Console.WriteLine(nameof(RenameSaveGameCommand));
            if (parameter is SaveGameViewModel ren)
                await RenameSaveGame(ren);
        }
        public async Task RenameSaveGame(SaveGameViewModel saveGame)
        {
            if (Saves.Contains(saveGame))
            {
                string oldTitle = saveGame.Title;
                string newTitle = await DialogDisplay.ShowDialog(new TextBoxDialogViewModel("Rename save game", string.Empty, oldTitle));
                if ((newTitle != null) && (newTitle != oldTitle) && (!(string.IsNullOrEmpty(newTitle) || string.IsNullOrWhiteSpace(newTitle))))
                {
                    saveGame.Rename(newTitle);
                }
            }
        }

        public async Task<SaveGameViewModel> CreateSaveGame(bool allowsCancel = true)
        {
            var saveGame = await DialogDisplay.ShowDialog(new NewSaveGameViewModel(SavesPath, allowsCancel));
            if (saveGame != null)
                Saves.Add(saveGame);
            return saveGame;
        }

        public async void PlayGameWithSave(object parameter)
        {
            if (parameter is SaveGameViewModel save)
            {

                string prefsDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarksporeData", "Preferences");
				string loginPropPath = Path.Combine(prefsDirPath, "login.prop");
                string loginPropText = $"UserName \"{save.Title}\"\n";
                Debug.WriteLine($"Launching Darkspore for save: '{save.Title}'");

                if (!Directory.Exists(prefsDirPath))
					Directory.CreateDirectory(prefsDirPath);
				File.WriteAllText(loginPropPath, loginPropText);
                //Debug.WriteLine($"path: '{loginPropPath}'\n\ntext:\n{loginPropText}");

                string gameBinPath = Path.Combine(GameInstallPath, "DarksporeBin");
                string gameExePath = Path.Combine(gameBinPath, "Darkspore_ReCapPatched.exe");

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
                var serverProcess = LocalServer.Instance.Start();


                ProcessStartInfo darksporeStartInfo = new ProcessStartInfo(gameExePath) //, "-nolauncher")
                {
                    WorkingDirectory = gameBinPath
                };
                using (Process darksporeProcess = new Process()
                {
                    StartInfo = darksporeStartInfo
                })
                {
                    //darksporeProcess.WaitForExit();
                    darksporeProcess.Start();
#if NET5_0_OR_GREATER
                    await darksporeProcess.WaitForExitAsync();
                    Debug.WriteLine("DARKSPORE EXITED");

                    serverProcess.Kill();
#endif
                }
            }
        }
    }
}
