using System;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI
{
    public class ScrollBarInset
        : AvaloniaObject
    {
        public static readonly AttachedProperty<double> VerticalProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, ScrollViewer, double>("Vertical", 0.0);
        public static double GetVertical(ScrollViewer control)
            => control.GetValue(VerticalProperty);
        public static void SetVertical(ScrollViewer control, double value)
            => control.SetValue(VerticalProperty, value);


        public static readonly AttachedProperty<double> HorizontalProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, ScrollViewer, double>("Horizontal", 0.0);
        public static double GetHorizontal(ScrollViewer control)
            => control.GetValue(HorizontalProperty);
        public static void SetHorizontal(ScrollViewer control, double value)
            => control.SetValue(HorizontalProperty, value);
    }
}
