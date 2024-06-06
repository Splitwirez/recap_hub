using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using static ReCap.CommonUI.WinUnmanagedMethods;

namespace ReCap.CommonUI
{
    public partial class WindowChromeAddon
        : AvaloniaObject
    {
        static readonly Version _WIN8_0 = new Version(6, 2, 9200);
        static readonly Version _WIN8_1 = new Version(6, 3, 9600);
        static readonly bool _ACTUALLY_USE_WIN8_TRANSPARENCY_HACK = Win8_ActuallyUseTransparencyHack();
        static bool Win8_ActuallyUseTransparencyHack()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;

            Version osVersion = OSInfo.Version;
            if (osVersion.Major > 6)
                return false;

            return osVersion >= (OSInfo.IsVersionDefinitelyAccurate ? _WIN8_0 : _WIN8_1);
        }
        static void UseTransparencyHackHintProperty_Changed(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (!_ACTUALLY_USE_WIN8_TRANSPARENCY_HACK)
                return;

            bool newValue = e.GetNewValue<bool>();
            if (GetActualUseTransparencyHack(sender) != newValue)
                SetActualUseTransparencyHack(sender, newValue);
        }


        static void ActualUseTransparencyHackProperty_Changed(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            var newValue = e.GetNewValue<bool>();
            OnActualUseTransparencyHackPropertyChanged(sender, newValue);
        }


        static void DwmDontExtendFrameIntoClientArea(IntPtr hWnd)
        {

            var margins = new MARGINS()
            {
                cxLeftWidth = 0,
                cyTopHeight = 0,
                cxRightWidth = 0,
                cyBottomHeight = 0,
            };
            var ret = DwmExtendFrameIntoClientArea(hWnd, ref margins);
            Debug.WriteLine($"{nameof(DwmExtendFrameIntoClientArea)}: {ret}");
        }
        static void OnActualUseTransparencyHackPropertyChanged(Window sender, bool newValue)
        {
            if (!sender.TryGetHWnd(out IntPtr hWnd))
                return;

            if (SetWindowCompositionAttributeForTransparency(hWnd, newValue))
            {
                Debug.WriteLine("SafeSetWindowCompositionAttribute");
                Dispatcher.UIThread.Post(() => DwmDontExtendFrameIntoClientArea(hWnd));
            }
        }

        static bool SetWindowCompositionAttributeForTransparency(IntPtr hWnd, bool transparent)
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);

            accent.AccentState = transparent
                ? AccentState.ACCENT_ENABLE_BLURBEHIND_BUT_ITS_PER_PIXEL_ALPHA_ON_WINDOWS_8
                : AccentState.ACCENT_DISABLED
            ;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            return SafeSetWindowCompositionAttribute(hWnd, ref data);
        }
        static bool SafeSetWindowCompositionAttribute(IntPtr hWnd, ref WindowCompositionAttributeData data)
            => SetWindowCompositionAttribute(hWnd, ref data) > 0;
    }
}
