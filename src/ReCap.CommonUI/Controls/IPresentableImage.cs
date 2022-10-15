using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReCap.CommonUI
{
    public interface IPresentableImage
    {
        void Render(ImagePresenter presenter, DrawingContext context);

        Size MeasureOverride(ImagePresenter presenter, Size availableSize, Size baseSize);

        event EventHandler<AvaloniaPropertyChangedEventArgs> AffectMeasure;
        event EventHandler<AvaloniaPropertyChangedEventArgs> AffectRender;
    }
}
