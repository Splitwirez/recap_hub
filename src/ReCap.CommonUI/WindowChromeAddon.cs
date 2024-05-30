using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public class WindowChromeAddon
        : AvaloniaObject
    {
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        static bool DefaultToLeftSideButtons => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("LeftSideButtons", DefaultToLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);


        static bool ShouldActuallyUseWin8TransparencyHack = new Func<bool>(() =>
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;
            
            if (!TryGetWindowsVersion(out Version osVersion))
                return false;

            if (osVersion.Major > 6)
                return false;

            return osVersion >= new Version(6, 2, 9200);
        })();
        static bool SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx)
        {
            osVersionInfoEx = new WinUnmanagedMethods.RTL_OSVERSIONINFOEX();
            return WinUnmanagedMethods.RtlGetVersion(ref osVersionInfoEx) == 0;
        }
        static bool TryGetWindowsVersion(out Version osVersion)
        {
            osVersion = default;
            try
            {
                if (!SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx))
                    return false;

                osVersion = new Version((int)osVersionInfoEx.dwMajorVersion, (int)osVersionInfoEx.dwMinorVersion, (int)osVersionInfoEx.dwBuildNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static readonly AttachedProperty<bool> UseWin8TransparencyHackProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("UseWin8TransparencyHack", false);
        public static bool GetUseWin8TransparencyHack(Window control)
            => control.GetValue(UseWin8TransparencyHackProperty);
        public static void SetUseWin8TransparencyHack(Window control, bool value)
            => control.SetValue(UseWin8TransparencyHackProperty, value);

        static WindowChromeAddon()
        {
            //UseWin8TransparencyHackProperty.Changed.AddClassHandler<Window>(UseWin8TransparencyHackProperty_Changed);
        }

        /*
        static void UseWin8TransparencyHackProperty_Changed(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.GetNewValue<bool>() && ShouldActuallyUseWin8TransparencyHack)
                OnWin8TransparencyHackPropertyChanged(sender);
        }

        static void OnWin8TransparencyHackPropertyChanged(Window sender)
        {
            if (!sender.TryGetHWnd(out IntPtr hWnd))
                return;


            var margins = new WinUnmanagedMethods.MARGINS()
            {
                cxLeftWidth = 0,
                cyTopHeight = 0,
                cxRightWidth = 0,
                cyBottomHeight = 0,
            };
            var ret = WinUnmanagedMethods.DwmExtendFrameIntoClientArea(hWnd, ref margins);
            System.Diagnostics.Debug.WriteLine($"{nameof(WinUnmanagedMethods.DwmExtendFrameIntoClientArea)}: {ret}");
        }
        */
    }
}
