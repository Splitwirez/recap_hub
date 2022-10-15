using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            set => RASIC(ref _saves, value);
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
        public GameConfigViewModel(string gameInstallPath, string savesPath)
            : base()
        {
            GameInstallPath = gameInstallPath;
            SavesPath = savesPath;
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
                        var save = new SaveGameViewModel();
                        save.ReadFromXml(f);
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
        }

        public async void NewSaveGame(object parameter)
        {
            await CreateSaveGame(true);
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

                string loginPropPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DarksporeData", "Preferences", "login.prop");
                string loginPropText = $"UserName \"{save.Title}\"\n";
                Debug.WriteLine($"Launching Darkspore for save: '{save.Title}'");

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


                ProcessStartInfo darksporeStartInfo = new ProcessStartInfo(gameExePath, "-nolauncher")
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
