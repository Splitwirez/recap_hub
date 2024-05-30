using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Reactive;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;

namespace ReCap.CommonUI
{
    public class TitleBarUnderlay
        : TemplatedControl
    {
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            
            if (TopLevel.GetTopLevel(this) is Window window)
                window.BeginMoveDrag(e);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (!(e.Root is Window window))
                return;

            //this[!HeightProperty] = window[!Window.ExtendClientAreaTitleBarHeightHintProperty];
            //this[!IsVisibleProperty] = window[!Window.IsExtendedIntoWindowDecorationsProperty];
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            //this[!HeightProperty] = null;
            //this[!IsVisibleProperty] = null;
        }

        protected override Type StyleKeyOverride => typeof(TitleBarUnderlay);
    }
}
