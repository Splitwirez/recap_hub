using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using ReCap.Hub.Data;
using ReCap.Hub.ViewModels;
using ReCap.Hub.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ReCap.Hub
{
    public class App : Application
    {
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

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                };

//#if BACKEND_POC
                LocalServer.ServerStarted += (s, e) =>
                {
                    if (OperatingSystem.IsWindows())
                    {
                        IntPtr cmd = WindowsPInvoke.GetConsoleWindow();
                        WindowsPInvoke.ShowWindow(cmd, 4);
                        if (desktop.MainWindow != null)
                        {
                            IntPtr mainWin = desktop.MainWindow.PlatformImpl.Handle.Handle;
                            if (WindowsPInvoke.GetWindowRect(mainWin, out WindowsPInvoke.RECT mainWinRect))
                            {
                                var flags = WindowsPInvoke.SetWindowPosFlags.IgnoreResize;
                                WindowsPInvoke.SetWindowPos(cmd, mainWin, mainWinRect.X, mainWinRect.Y, 0, 0, flags);
                            }
                        }
                    }

                    desktop.MainWindow?.Hide();
                };

                LocalServer.ServerExited += (s, e) => Dispatcher.UIThread.Post(() =>
                {
#if PERSIST_AFTER_GAME_EXIT
                    desktop.MainWindow?.Show();

                    if (OperatingSystem.IsWindows())
                    {
                        IntPtr cmd = WindowsPInvoke.GetConsoleWindow();
                        WindowsPInvoke.ShowWindow(cmd, 0);
                        if (desktop.MainWindow != null)
                        {
                            IntPtr mainWin = desktop.MainWindow.PlatformImpl.Handle.Handle;
                            if (WindowsPInvoke.GetWindowRect(cmd, out WindowsPInvoke.RECT cmdRect))
                            {
                                var flags = WindowsPInvoke.SetWindowPosFlags.IgnoreResize | WindowsPInvoke.SetWindowPosFlags.ShowWindow;
                                    // | PInvoke.SetWindowPosFlags.HideWindow); //| PInvoke.SetWindowPosFlags.DoNotActivate | PInvoke.SetWindowPosFlags.DoNotSendChangingEvent | PInvoke.SetWindowPosFlags.DoNotRedraw); // | PInvoke.SetWindowPosFlags.ShowWindow);
                                
                                WindowsPInvoke.SetWindowPos(mainWin, cmd, cmdRect.X, cmdRect.Y, 0, 0, flags);
                            }
                        }
                    }
                    else
                    {
                        Dispatcher.UIThread.Post(() => desktop.MainWindow?.Activate(), DispatcherPriority.Render);
                    }
#else
                    Dispatcher.UIThread.Post(() => Environment.Exit(0));
#endif
                });


                if (HubData.Instance.GameConfigs.Count == 0)
                {
                    desktop.MainWindow?.Hide();
                    //string gamePath = null;
                    //string savesPath = null;

                    //TEMPORARY
                    //gamePath = @"E:\Programs (x86)\Electronic Arts\Darkspore";
                    ////////savesPath = Path.Combine(LocalServer.Instance.ServerDirPath, "storage", "users");


                    Task.Run(async () =>
                    {
                        var gamePaths = await DialogDisplay.ShowDialog(new LocateDarksporeViewModel(false));

                        GameConfigViewModel gameConfig = 
                            new GameConfigViewModel(gamePaths.darksporeInstallPath, gamePaths.wineExecutable, gamePaths.winePrefix)
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
//#endif
            }
        }

//#if BACKEND_POC
        void ShowMainWindow(IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow.DataContext = new MainWindowViewModel(HubData.Instance.GameConfigs[0]);
            desktop.MainWindow?.Show();
        }
//#endif
    }
}