using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public partial class WindowChromeAddon
        : AvaloniaObject
    {
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);




        static bool DefaultToLeftSideButtons => OSInfo.IsMacOS;
        public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("LeftSideButtons", DefaultToLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);




        public static readonly AttachedProperty<bool> UseTransparencyHackHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("UseTransparencyHackHint", false);
        public static bool GetUseTransparencyHackHint(Window control)
            => control.GetValue(UseTransparencyHackHintProperty);
        public static void SetUseTransparencyHackHint(Window control, bool value)
            => control.SetValue(UseTransparencyHackHintProperty, value);




        public static readonly AttachedProperty<bool> ActualUseTransparencyHackProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ActualUseTransparencyHack", false);
        public static bool GetActualUseTransparencyHack(Window control)
            => control.GetValue(ActualUseTransparencyHackProperty);
        static void SetActualUseTransparencyHack(Window control, bool value)
            => control.SetValue(ActualUseTransparencyHackProperty, value);




        static WindowChromeAddon()
        {
            UseTransparencyHackHintProperty.Changed.AddClassHandler<Window>(UseTransparencyHackHintProperty_Changed);
            ActualUseTransparencyHackProperty.Changed.AddClassHandler<Window>(ActualUseTransparencyHackProperty_Changed);
        }
    }
}
