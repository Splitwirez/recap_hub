using Avalonia;
using System;
using System.Diagnostics;

namespace ReCap.Hub
{
    public partial class App
        : Application
    {
        static bool GetShouldUseManagedWindowDecorationsByDefault_Linux()
        {
            Debug.WriteLine($"{nameof(GetShouldUseManagedWindowDecorationsByDefault_Linux)}()");
            string xdgCurrentDesktop = Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP");
            if (xdgCurrentDesktop != null)
            {
                string[] desktopEnvs = xdgCurrentDesktop.Contains(':')
                    ? xdgCurrentDesktop.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    : new[]{ xdgCurrentDesktop }
                ;
                
                foreach (string desktopEnv in desktopEnvs)
                {
                    if (desktopEnv == "KDE")
                    {
                        Debug.WriteLine("KDE - default to system decorations");
                        return false;
                    }

                    if ((desktopEnv == "GNOME") || (desktopEnv == "GNOME-Flashback"))
                    {
                        Debug.WriteLine("GNOME - default to managed decorations");
                        return true;
                    }
                    
                    // TODO: Others
                    // https://wiki.archlinux.org/title/Environment_variables#Examples
                }
            }

            //TODO: Should fallback be system or managed decorations?
            Debug.WriteLine("FALLBACK CASE");
            return false;
        }
    }
}