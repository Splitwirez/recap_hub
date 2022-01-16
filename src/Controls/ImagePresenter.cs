using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ReCap.Hub.Controls
{
    public class ImagePresenter : Control
    {
        /// <summary>
        /// Defines the <see cref="Source"/> property.
        /// </summary>
        public static readonly StyledProperty<IPresentableImage> SourceProperty =
            AvaloniaProperty.Register<ImagePresenter, IPresentableImage>(nameof(Source));
        /// <summary>
        /// Gets or sets the image that will be displayed.
        /// </summary>
        [Content]
        public IPresentableImage Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }



        static ImagePresenter()
        {
            AffectsRender<ImagePresenter>(SourceProperty);
            AffectsMeasure<ImagePresenter>(SourceProperty);
            SourceProperty.Changed.AddClassHandler<ImagePresenter>((s, e) => s.OnSourceChanged());
        }

        void OnSourceChanged()
        {
            if (_src != null)
            {
                _src.AffectMeasure -= Src_AffectMeasure;
                _src.AffectRender -= Src_AffectRender;
            }

            _src = Source;

            if (_src != null)
            {
                _src.AffectMeasure += Src_AffectMeasure;
                _src.AffectRender += Src_AffectRender;
            }
        }

        private void Src_AffectRender(object? sender, AvaloniaPropertyChangedEventArgs e)
            => InvalidateVisual();

        private void Src_AffectMeasure(object? sender, AvaloniaPropertyChangedEventArgs e)
            => InvalidateMeasure();

        public ImagePresenter()
            : base()
        {
            OnSourceChanged();
        }


        IPresentableImage _src = null;


        public override void Render(DrawingContext context)
        {
            _src?.Render(this, context);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size baseSize = base.MeasureOverride(availableSize);
            if (_src == null)
                return baseSize;

            return _src.MeasureOverride(this, availableSize, baseSize);
        }
    }
}
