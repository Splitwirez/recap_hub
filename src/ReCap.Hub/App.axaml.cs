using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReCap.CommonUI;
using ReCap.Hub.Data;
using ReCap.Hub.Localization;
using ReCap.Hub.ViewModels;
using ReCap.Hub.Views;

namespace ReCap.Hub
{
    public partial class App
        : Application
    {
        Window _mainWindow = null;

        public override void Initialize()
        {
            /*var bodyFont = new FontFamily("avares://ReCap.Hub/Res/UI/Font/Body/SingleBold#RobotoCondensed");
            FontManager.Current.GetOrAddGlyphTypeface(new Typeface(bodyFont, weight: FontWeight.Black));
            FontManager.Current.GetOrAddGlyphTypeface(new Typeface(bodyFont, weight: FontWeight.DemiBold));
            FontManager.Current.GetOrAddGlyphTypeface(new Typeface(bodyFont, weight: FontWeight.SemiBold));
            
            var bodyFontLight = new FontFamily("avares://ReCap.Hub/Res/UI/Font/Body/SingleLight#Roboto Condensed");
            FontManager.Current.GetOrAddGlyphTypeface(new Typeface(bodyFontLight, weight: FontWeight.SemiLight));
            
            Resources.Add("BodyFont", bodyFont);*/
            AvaloniaXamlLoader.Load(this);
        }

        ReCapTheme _reCapTheme = null;
        public bool UseManagedWindowChrome
        {
            get => _reCapTheme?.UseManagedChrome ?? false;
            set
            {
                if (_reCapTheme != null)
                    _reCapTheme.UseManagedChrome = value;
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var dict = UILanguageManager.Instance.Dictionary;
            Resources.MergedDictionaries.Add(dict);

            base.OnFrameworkInitializationCompleted();
            var styles = Styles;
            foreach (var style in styles)
            {
                if (style is ReCapTheme reCapTheme)
                {
                    _reCapTheme = reCapTheme;
                    break;
                }
            }
            // TODO: Restore once managed decorations actually work
            //UseManagedWindowChrome = GetShouldUseManagedWindowDecorationsByDefault();
            
            var lifetime = ApplicationLifetime;
            if (lifetime == null)
                throw new NullReferenceException($"{nameof(lifetime)} must not be null.");
            
            if (!(lifetime is IClassicDesktopStyleApplicationLifetime desktop))
                throw new Exception($"{nameof(lifetime)} must implement {nameof(IClassicDesktopStyleApplicationLifetime)}, but was of type {lifetime.GetType().FullName} which doesn't.");
            
            
            _mainWindow = new MainWindow()
            {
            };
            desktop.MainWindow = _mainWindow;

            LocalServer.ServerStarted += OnLocalServerStarted;
            LocalServer.ServerExited += OnLocalServerExited;
            GameLaunchService.GameExited += OnGameExited;


            int gameConfigsCount = HubData.Instance.GameConfigs.Count;
            if (gameConfigsCount <= 0)
            {
                _mainWindow?.Hide();
                //string gamePath = null;
                //string savesPath = null;

                //TEMPORARY
                //gamePath = @"E:\Programs (x86)\Electronic Arts\Darkspore";
                ////////savesPath = Path.Combine(LocalServer.Instance.ServerDirPath, "storage", "users");


                Task.Run(async () =>
                {
                    var gamePaths = await DialogDisplay.ShowDialog(new LocateDarksporeViewModel(false));

                    string gameVersion = null;

                    string installPath = gamePaths.DarksporeInstallPath;
                    string exePath = Path.Combine(installPath, "DarksporeBin", "Darkspore.exe");
                    if (File.Exists(exePath))
                    {
                        var pe = new PeNet.PeFile(exePath);
                        var strFileInfo = pe.Resources.VsVersionInfo.StringFileInfo;
                        var stringTables = strFileInfo.StringTable;

                        if (stringTables.Length > 0)
                            gameVersion = stringTables[0].ProductVersion;

                        if (string.IsNullOrEmpty(gameVersion) || string.IsNullOrWhiteSpace(gameVersion))
                        {
                            Debug.WriteLine("PeFile failed to retrieve exe version");

                            // try our luck by falling back to FileVersionInfo (which currently only works on Windows)
                            var gameVersionInfo = FileVersionInfo.GetVersionInfo(exePath);
                            gameVersion = gameVersionInfo.ProductVersion;
                        }
                    }
                    if (string.IsNullOrEmpty(gameVersion) || string.IsNullOrWhiteSpace(gameVersion))
                        gameVersion = "?.?.?.?";

                    string gameConfigName = $"Darkspore v{gameVersion}";

                    GameConfigViewModel gameConfig =
                        new GameConfigViewModel(installPath, gamePaths.WineExecutable, gamePaths.WinePrefix)
                        {
                            Title = gameConfigName
                        }
                    ;
                    HubData.Instance.GameConfigs.Add(gameConfig);

                    if (gameConfig.Saves.Count <= 0)
                    {
                        var saveGame = await gameConfig.CreateSaveGame(false);
                        gameConfig.SelectedSave = saveGame;
                    }

                    HubData.Instance.Save();
                    Dispatcher.UIThread.Post(() => ShowMainWindow(desktop));
                });
            }
            else if ((gameConfigsCount == 1) && TimeHelper.TryGetNewest(HubData.Instance.GameConfigs, vm => vm.LastLaunchTime, out GameConfigViewModel gameConfig) && (gameConfig.Saves.Count <= 0))
            {
                Task.Run(async () =>
                {
                    var saveGame = await gameConfig.CreateSaveGame(false);
                    gameConfig.SelectedSave = saveGame;

                    HubData.Instance.Save();
                    Dispatcher.UIThread.Post(() => ShowMainWindow(desktop));
                });
            }
            else
            {
                ShowMainWindow(desktop);
            }
        }

        void ShowMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow.DataContext = new MainViewModel();
            _mainWindow?.Show();
        }

        public static bool GetShouldUseManagedWindowDecorationsByDefault()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetShouldUseManagedWindowDecorationsByDefault_Windows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetShouldUseManagedWindowDecorationsByDefault_Linux();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return GetShouldUseManagedWindowDecorationsByDefault_macOS();
            }
            
            return false;
        }
    }
}