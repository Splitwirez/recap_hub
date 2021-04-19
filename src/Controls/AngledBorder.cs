using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;

namespace ReCap.Hub.Controls
{
    public partial class AngledBorder : Border
    {
        PathGeometry _geometry = new PathGeometry();
        


        public AngledBorder() : base()
        {
            var ctrl = (this as Control);
            
            ctrl.LayoutUpdated += (s, e) => UpdateGeometry();
        }

        void UpdateGeometry()
        {
            double width = Bounds.Width - 1;
            double height = Bounds.Height - 1;

            double minDimen = Math.Min(width, height);
            
            CornerRadius radius = CornerRadius;
            double tl = Math.Min(radius.TopLeft + 1, minDimen);
            double tr = Math.Min(radius.TopRight + 1, minDimen);
            double br = Math.Min(radius.BottomRight + 1, minDimen);
            double bl = Math.Min(radius.BottomLeft + 1, minDimen);
            
            _geometry.Figures = new PathFigures()
            {
                new PathFigure()
                {
                    StartPoint = new Point(1, tl),
                    IsClosed = true,
                    Segments = 
                    {
                        new LineSegment()
                        {
                            Point = new Point(tl, 1)
                        },
                        new LineSegment()
                        {
                            Point = new Point(width - tr, 1)
                        },
                        new LineSegment()
                        {
                            Point = new Point(width, tr)
                        },
                        new LineSegment()
                        {
                            Point = new Point(width, height - br)
                        },
                        new LineSegment()
                        {
                            Point = new Point(width - br, height)
                        },
                        new LineSegment()
                        {
                            Point = new Point(bl, height)
                        },
                        new LineSegment()
                        {
                            Point = new Point(1, height - bl)
                        }
                    }
                }
            };
        }

        public override void Render(DrawingContext context)
        {
            var brdThck = BorderThickness;

            //context.DrawRectangle(new SolidColorBrush(Avalonia.Media.Colors.Yellow), null, Bounds.WithX(0).WithY(0));
            double borderThickness = (brdThck.Left + brdThck.Top + brdThck.Right + brdThck.Bottom) / 4;
            context.DrawGeometry(Background, new Pen()
            {
                Brush = BorderBrush,
                Thickness = borderThickness
            }, _geometry);
        }
    }
}