using System;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public partial class CaptionButtonsSizeBridge
        : Panel
    {
        public static readonly AttachedProperty<double> CaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<CaptionButtonsSizeBridge, Window, double>("CaptionButtonsWidth", 0.0f);
        public static double GetCaptionButtonsWidth(Window control)
            => control.GetValue(CaptionButtonsWidthProperty);
        public static void SetCaptionButtonsWidth(Window control, double value)
            => control.SetValue(CaptionButtonsWidthProperty, value);


        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            RefreshCaptionButtonsWidth(e.NewSize.Width);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            RefreshCaptionButtonsWidth(Bounds.Width);
        }

        void RefreshCaptionButtonsWidth(double newValue)
        {
            if (TopLevel.GetTopLevel(this) is Window window)
                SetCaptionButtonsWidth(window, newValue);
        }
    }
}
