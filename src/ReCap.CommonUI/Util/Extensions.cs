using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> appendTo, IEnumerable<T> appendFrom)
            => appendTo.AppendRange(appendFrom.ToArray());

        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> appendTo, params T[] appendFrom)
        {
            var ret = appendTo.ToList();
            ret.AddRange(appendFrom);
            return ret;
        }




        public static bool TryGetOldValue<T>(this AvaloniaPropertyChangedEventArgs e, out T oldValue)
        {
            if (e == null)
            {
                oldValue = default;
                return false;
            }

            oldValue = e.GetOldValue<T>();
            return true;
        }

        public static bool TryGetNewValue<T>(this AvaloniaPropertyChangedEventArgs e, out T newValue)
        {
            if (e == null)
            {
                newValue = default;
                return false;
            }

            newValue = e.GetNewValue<T>();
            return true;
        }

        public static bool TryGetOldAndNewValue<T>(this AvaloniaPropertyChangedEventArgs e, out T oldValue, out T newValue)
        {
            bool oldRet = e.TryGetOldValue(out oldValue);
            bool newRet = e.TryGetNewValue(out newValue);
            return oldRet && newRet;
        }
    }
}