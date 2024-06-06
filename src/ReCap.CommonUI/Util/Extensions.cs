using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;

namespace ReCap.CommonUI
{
    public static partial class Extensions
    {
        public static bool TryGetHWnd(this Window win, out IntPtr hWnd)
        {
            hWnd = IntPtr.Zero;
            if (win == null)
                return false;
            
            var platHandle = win.TryGetPlatformHandle();
            if (platHandle == null)
                return false;
            
            IntPtr? hWndMaybe = platHandle.Handle;
            if (!hWndMaybe.HasValue)
                return false;
            
            hWnd = hWndMaybe.Value;
            return true;
        }

        public static void MakeControlTypeNonInteractive<T>()
            where T : AvaloniaObject
        {
            InputElement.FocusableProperty.OverrideDefaultValue<T>(false);
            InputElement.IsTabStopProperty.OverrideDefaultValue<T>(false);
            ContentPresenter.RecognizesAccessKeyProperty.OverrideDefaultValue<T>(false);
        }
    }
}