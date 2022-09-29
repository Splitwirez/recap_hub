using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Utils;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace ReCap.Hub.Controls
{
    public abstract class AngledBorderBase : Decorator
    {
        /// <summary>
        /// Defines the <see cref="Background"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<AngledBorderBase>();
        /// <summary>
        /// Gets or sets a brush with which to paint the background.
        /// </summary>
        public IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BorderBrush"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush> BorderBrushProperty =
            Border.BorderBrushProperty.AddOwner<AngledBorderBase>();
        /// <summary>
        /// Gets or sets a brush with which to paint the border.
        /// </summary>
        public IBrush BorderBrush
        {
            get => GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        

        /// <summary>
        /// Defines the <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty =
            Shape.StrokeThicknessProperty.AddOwner<AngledBorderBase>();
        /// <summary>
        /// Gets or sets the width of the shape outline.
        /// </summary>
        public double StrokeThickness
        {
            get => GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="InnerGlowSize"/> property.
        /// </summary>
        public static readonly StyledProperty<double> InnerGlowSizeProperty =
            AvaloniaProperty.Register<AngledBorderBase, double>(nameof(InnerGlowSize), -1);
        
        public double InnerGlowSize
        {
            get => GetValue(InnerGlowSizeProperty);
            set => SetValue(InnerGlowSizeProperty, value);
        }

        /// <summary>
        /// Defines the <see cref="InnerGlowColor"/> property.
        /// </summary>
        public static readonly StyledProperty<Color> InnerGlowColorProperty =
            AvaloniaProperty.Register<AngledBorderBase, Color>(nameof(InnerGlowColor));
        
        public Color InnerGlowColor
        {
            get => GetValue(InnerGlowColorProperty);
            set => SetValue(InnerGlowColorProperty, value);
        }




        BoxShadow _boxShadow = new BoxShadow()
        {
            OffsetX = 0,
            OffsetY = 0,
            IsInset = true,
        };
        BoxShadows _boxShadows = new BoxShadows();
        Geometry _fillGeometry = new StreamGeometry();
        Geometry _strokeGeometry = new StreamGeometry();
        RoundedRect _rrect = new RoundedRect(new Rect(0, 0, 3, 3), 0);
        
        Thickness _strokeThickness = new Thickness(0);

        

        static AngledBorderBase()
        {
            Action<AngledBorderBase, AvaloniaPropertyChangedEventArgs> changedHandler = (s, e) => s.UpdateGeometry();

            StrokeThicknessProperty.Changed.AddClassHandler<AngledBorderBase>((s, e) => 
            {
                s._strokeThickness = new Thickness((double)(e.NewValue));
                changedHandler(s, e);
            });
            
            
            InnerGlowColorProperty.Changed.AddClassHandler<AngledBorderBase>((s, e) => s._boxShadow.Color = ((e.NewValue != null) && (e.NewValue is Color color)) ? color : Colors.Transparent);
            InnerGlowSizeProperty.Changed.AddClassHandler<AngledBorderBase>((s, e) => s._boxShadow.Blur = ((e.NewValue != null) && (e.NewValue is double radius)) ? radius : -1);

            
            AffectsRender<AngledBorderBase>(
                BackgroundProperty,
                BorderBrushProperty,
                StrokeThicknessProperty,
                InnerGlowColorProperty,
                InnerGlowSizeProperty);
        }


        public AngledBorderBase() : base()
        {
            //_boxShadows = new BoxShadows(_boxShadow);
            
            var ctrl = (this as Control);
            
            ctrl.LayoutUpdated += (s, e) => UpdateGeometry();
            ctrl.AttachedToVisualTree += (s, e) => UpdateGeometry();
        }

        void OnBorderThicknessChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var nv = e.NewValue;
            //_averageBorderThickness = ((nv != null) && (nv is Thickness brdThck)) ? ((brdThck.Left + brdThck.Top + brdThck.Right + brdThck.Bottom) / 4) : 0;
        }

        protected void UpdateGeometry()
        {
            (_fillGeometry, _strokeGeometry, _rrect) = RefreshGeometry();

            
            double minDimen = InnerGlowSize;
            double width = Math.Round(Bounds.Width);
            double height = Math.Round(Bounds.Height);

            minDimen = ((minDimen >= 0) ? minDimen : 1) * Math.Min(width, height);
            
            /*double blurBase = minDimen / 2;
            double spreadBase = minDimen / 3;
            _boxShadow.Blur = blurBase + spreadBase;
            _boxShadow.Spread = spreadBase;*/
            
            double blurBase = minDimen / 2;
            double spreadBase = minDimen / 9;
            _boxShadow.Blur = blurBase - spreadBase;
            _boxShadow.Spread = spreadBase;
        }
        
        protected abstract (Geometry, Geometry, RoundedRect) RefreshGeometry();
        

        public override void Render(DrawingContext context)
        {
            //Console.WriteLine($"Rendering...\n\t_fillGeometry?: {_fillGeometry != null}\n\t_strokeGeometry?: {_strokeGeometry != null}");
            

            using (var idk2 = context.PushGeometryClip(_fillGeometry))
            {
                context.PlatformImpl.DrawRectangle(Background, null, _rrect, new BoxShadows(_boxShadow));
            }


            if (StrokeThickness > 0)
            {
                context.DrawGeometry
                (
                    BorderBrush //null
                    , null/*new Pen()
                    {
                        Brush = ,
                        Thickness = _averageBorderThickness
                    }*/
                    , _strokeGeometry
                );
            }
        }

        static readonly Thickness ZERO = new Thickness(0);
        protected override Size MeasureOverride(Size availableSize)
        {
            return LayoutHelper.MeasureChild(Child, availableSize, Padding, ZERO);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return LayoutHelper.ArrangeChild(Child, finalSize, Padding, ZERO);
        }
    }
}