using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Avalonia.Controls;


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
    }
}