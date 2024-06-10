using Avalonia;
using ReCap.CommonUI;
using ReCap.Hub.Data;
using ReCap.Hub.ViewModels;
using ReCap.Hub.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace ReCap.Hub
{
    public partial class App : Application
    {
        void OnLocalServerStarted(object sender, EventArgs e)
        {
            if (OperatingSystem.IsWindows())
                    ServerStarted_Windows();

                _mainWindow?.Hide();
        }


        void OnLocalServerExited(object sender, EventArgs e)
            => Dispatcher.UIThread.Post(() =>
        {
#if EXIT_HUB_AFTER_GAME_EXIT
                Dispatcher.UIThread.Post(() => Environment.Exit(0));
#else
                ShowMainWindow(HubData.Instance.AutoCloseServer);
#endif
        });

        void OnGameExited(object sender, EventArgs e)
        {
            if (HubData.Instance.AutoCloseServer)
                return;
            ShowMainWindow(false);
        }

        void ShowMainWindow(bool closeServer)
        {
            _mainWindow?.Show();

            if (closeServer && OperatingSystem.IsWindows())
            {
                ServerExited_Windows();
            }
            else
            {
                Dispatcher.UIThread.Post(() => _mainWindow?.Activate(), DispatcherPriority.Render);
            }
        }
    }
}