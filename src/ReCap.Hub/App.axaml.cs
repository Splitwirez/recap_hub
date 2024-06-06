using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReCap.Hub.Data;
using ReCap.Hub.ViewModels;
using ReCap.Hub.Views;
using System;
using System.Threading.Tasks;
using ReCap.CommonUI;
using System.Runtime.InteropServices;
using System.Diagnostics;

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


            if (HubData.Instance.GameConfigs.Count == 0)
            {
                _mainWindow?.Hide();
                //string gamePath = null;
                //string savesPath = null;

                //TEMPORARY
                //gamePath = @"E:\Programs (x86)\Electronic Arts\Darkspore";
                ////////savesPath = Path.Combine(LocalServer.Instance.ServerDirPath, "storage", "users");


                Task.Run(async () =>
                {
                    var gamePaths = await DialogDisplay.ShowDialog<DarksporeInstallPaths>(new LocateDarksporeViewModel(false));

                    GameConfigViewModel gameConfig = 
                        new GameConfigViewModel(gamePaths.DarksporeInstallPath, gamePaths.WineExecutable, gamePaths.WinePrefix)
                        //new GameConfigViewModel(gamePath, savesPath)
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
            else
                ShowMainWindow(desktop);
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