using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Runtime.InteropServices;

namespace ReCap.Hub.Controls
{
    public partial class TitleBar2 : ContentControl
    {
        static bool DefaultToLeftSideButtons => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly StyledProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.Register<TitleBar2, bool>(nameof(LeftSideButtons), defaultValue: DefaultToLeftSideButtons);
        
        public bool LeftSideButtons
        {
            get => GetValue(LeftSideButtonsProperty);
            set => SetValue(LeftSideButtonsProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            e.NameScope.Find<Button>("PART_CloseButton").Click += (s, a) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.Close();
            };

            e.NameScope.Find<Button>("PART_MaximizeButton").Click += (s, a) => ToggleMaximize();

            e.NameScope.Find<Button>("PART_MinimizeButton").Click += (s, a) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.WindowState = WindowState.Minimized;
            };

            var dragGrip = e.NameScope.Find<TemplatedControl>("PART_DragGrip");
            
            dragGrip.PointerPressed += (s, a) =>
            {
                if (a.ClickCount <= 1)
                {
                    if (VisualRoot.GetVisualRoot() is Window win)
                        win.BeginMoveDrag(a);
                }
            };

            dragGrip.DoubleTapped += (s, a) => ToggleMaximize();
        }

        void ToggleMaximize()
        {
            if (VisualRoot.GetVisualRoot() is Window win)
            {
                if (win.WindowState == WindowState.Maximized)
                    win.WindowState = WindowState.Normal;
                else
                    win.WindowState = WindowState.Maximized;
            }
        }
    }
}