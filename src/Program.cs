using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;

namespace ReCap.Hub
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            if (RunningOn.Windows)
            {
                IntPtr console = PInvoke.GetConsoleWindow();
                
                PInvoke.GetWindowThreadProcessId(console, out uint consolePid);
                if (Process.GetCurrentProcess().Id == consolePid)
                {
                    if (PInvoke.IsWindow(console))
                        PInvoke.ShowWindow(console, 0);
                }
            }
            
            
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
