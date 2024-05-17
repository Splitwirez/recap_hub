using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public class ChromeHelper : AvaloniaObject
    {
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<ChromeHelper, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        static bool DefaultToLeftSideButtons => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<ChromeHelper, Window, bool>("LeftSideButtons", DefaultToLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);


        /*public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<ChromeHelper, Window, bool>("LeftSideButtons", DefaultToLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);*/


        public static readonly AttachedProperty<bool> UseTitleBarHackProperty =
            AvaloniaProperty.RegisterAttached<ChromeHelper, Window, bool>("UseTitleBarHack", false);
        public static bool GetUseTitleBarHack(Window control)
            => control.GetValue(UseTitleBarHackProperty);
        public static void SetUseTitleBarHack(Window control, bool value)
            => control.SetValue(UseTitleBarHackProperty, value);

        static ChromeHelper()
        {
            UseTitleBarHackProperty.Changed.AddClassHandler<Window>(UseTitleBarHackChanged);
        }

        static void UseTitleBarHackChanged(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;
            
            if (e.GetNewValue<bool>())
            {
                //var type = sender.PlatformImpl.GetType();
                //type.GetField()
                
            }
        }
    }
}
