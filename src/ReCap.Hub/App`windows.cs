using Avalonia;
using ReCap.CommonUI;
using System;

namespace ReCap.Hub
{
    public partial class App
        : Application
    {
#pragma warning disable CA1416
        void ServerStarted_Windows()
        {
            IntPtr cmd = WindowsPInvoke.GetConsoleWindow();
            WindowsPInvoke.ShowWindow(cmd, 4);
            
            if (!_mainWindow.TryGetHWnd(out IntPtr hMainWnd))
                return;
            
            if (WindowsPInvoke.GetWindowRect(hMainWnd, out WindowsPInvoke.RECT mainWinRect))
            {
                var flags = WindowsPInvoke.SetWindowPosFlags.IgnoreResize;
                WindowsPInvoke.SetWindowPos(cmd, hMainWnd, mainWinRect.X, mainWinRect.Y, 0, 0, flags);
            }
        }

        void ServerExited_Windows()
        {
            IntPtr hCmdWnd = WindowsPInvoke.GetConsoleWindow();
            WindowsPInvoke.ShowWindow(hCmdWnd, 0);
            if (!_mainWindow.TryGetHWnd(out IntPtr hMainWnd))
                return;
            
            if (WindowsPInvoke.GetWindowRect(hCmdWnd, out WindowsPInvoke.RECT cmdRect))
            {
                var flags = WindowsPInvoke.SetWindowPosFlags.IgnoreResize | WindowsPInvoke.SetWindowPosFlags.ShowWindow;
                    // | PInvoke.SetWindowPosFlags.HideWindow); //| PInvoke.SetWindowPosFlags.DoNotActivate | PInvoke.SetWindowPosFlags.DoNotSendChangingEvent | PInvoke.SetWindowPosFlags.DoNotRedraw); // | PInvoke.SetWindowPosFlags.ShowWindow);
                
                WindowsPInvoke.SetWindowPos(hMainWnd, hCmdWnd, cmdRect.X, cmdRect.Y, 0, 0, flags);
            }
        }
#pragma warning restore CA1416

        static bool GetShouldUseManagedWindowDecorationsByDefault_Windows()
        {
            return true;
        }
    }
}