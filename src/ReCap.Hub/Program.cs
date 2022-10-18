using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReCap.Hub.Data;

namespace ReCap.Hub
{
    partial class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            if (OperatingSystem.IsWindows())
            {
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {

                };

                IntPtr console = WindowsPInvoke.GetConsoleWindow();

                if (!WindowsPInvoke.IsWindow(console))
                {
                    bool what = WindowsPInvoke.AllocConsole();
                    console = WindowsPInvoke.GetConsoleWindow();
                }
                else
                {
                    WindowsPInvoke.GetWindowThreadProcessId(console, out uint consolePid);
                    if (Process.GetCurrentProcess().Id != consolePid)
                    {
                        console = IntPtr.Zero;
                    }
                }

                WindowsPInvoke.ShowWindow(console, 0);
            }
            else if (OperatingSystem.IsLinux())
            {
                NMessageBox.DebugShow(IntPtr.Zero, "bottom text", "top text", NMessageBoxButtons.CancelTryagainContinue);
            }
            
            
            if (!CommandLine.Instance.MainParseCommandLine(args, out IEnumerable<Exception> exceptions))
            {
                //TODO: Do something useful with exceptions, if there are any
            }
            
            
            if (CommandLine.Instance.ShowGUI)
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }

            if (CommandLine.Instance.ExitCode < 0)
            {
                Environment.Exit(CommandLine.Instance.ExitCode);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .With(new Win32PlatformOptions()
                {
                    OverlayPopups = true
                }).With(new X11PlatformOptions()
                {
                    OverlayPopups = true
                });
        }
    }
}
