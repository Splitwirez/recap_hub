using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ReCap.CommonUI
{
    public class AngledBorder
        : AngledBorderBase
    {
        /// <summary>
        /// Defines the <see cref="CornerRadius"/> property.
        /// </summary>
        public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
            Border.CornerRadiusProperty.AddOwner<AngledBorder>();
        /// <summary>
        /// Gets or sets the radius of the border rounded corners.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        static AngledBorder()
        {
            AffectsGeometry<AngledBorder>(CornerRadiusProperty);
            AffectsRender<AngledBorder>(CornerRadiusProperty);
        }

        protected override void RefreshGeometry(out Geometry fillGeometry, out Geometry strokeGeometry, out RoundedRect glowRect)
        {
            //Console.WriteLine($"Updating geometries...");
            double width = Math.Round(Bounds.Width);
            double height = Math.Round(Bounds.Height);

            double minDimen = Math.Min(width, height);
            
            CornerRadius radius = CornerRadius;
            double tl = Math.Min(radius.TopLeft, minDimen);
            double tr = Math.Min(radius.TopRight, minDimen);
            double br = Math.Min(radius.BottomRight, minDimen);
            double bl = Math.Min(radius.BottomLeft, minDimen);
            
            /*tl = Math.Round(tl, 0);
            tr = Math.Round(tr, 0);
            br = Math.Round(br, 0);
            bl = Math.Round(bl, 0);*/
            
            double strokeThickness = StrokeThickness;
            double borderBothSides = strokeThickness * 2;
            double fillWidth = width - borderBothSides;
            double fillHeight = height - borderBothSides;
            var rect = new Rect(strokeThickness, strokeThickness, fillWidth, fillHeight);
            glowRect = new RoundedRect(rect, tl, tr, br, bl);
            
            var fillGeom = new StreamGeometry();
            using (var ctx = fillGeom.Open())
            {
                ctx.CreateGeometry(strokeThickness, strokeThickness, fillWidth, fillHeight, tl, tr, br, bl, true);
            }
            fillGeometry = fillGeom;

            /*double realAverageBorderThickness = _averageBorderThickness;
            _averageBorderThickness = 4;
            borderBothSides = _averageBorderThickness * 2;
            fillWidth = width - borderBothSides;
            fillHeight = height - borderBothSides;*/

            strokeGeometry = null;
            if (strokeThickness > 0)
            {
                double diagonalDiff = strokeThickness / 2;

                tl = (tl > 0) ? Math.Round(tl + diagonalDiff, 0) : 0;
                tr = (tr > 0) ? Math.Round(tr + diagonalDiff, 0) : 0;
                br = (br > 0) ? Math.Round(br + diagonalDiff, 0) : 0;
                bl = (bl > 0) ? Math.Round(bl + diagonalDiff, 0) : 0;
                var strokeOuterGeometry = new StreamGeometry();
                using (var ctx = strokeOuterGeometry.Open())
                {
                    ctx.CreateGeometry(0, 0, width, height, tl, tr, br, bl, true);
                    //////ctx.BeginFigure(new Point(0 + 1, 0 + tl), true);
                    //////ctx.TraverseGeometry(0, 0, width, height, tl, tr, br, bl, true);
                    //ctx.TraverseGeometry(_averageBorderThickness, _averageBorderThickness, fillWidth, fillHeight, tl - _averageBorderThickness, tr - _averageBorderThickness, br - _averageBorderThickness, bl - _averageBorderThickness, false);
                    //////ctx.EndFigure(true);
                }

                //_averageBorderThickness = realAverageBorderThickness;
                //strokeGeometry = new CombinedGeometry(GeometryCombineMode.Xor, strokeOuterGeometry, fillGeometry/*, new TranslateTransform(0, 0)*/);
                strokeGeometry = strokeOuterGeometry;
            }
        }
    }

    public static partial class AngledCorners
    {
        public static void CreateGeometry(this StreamGeometryContext ctx, double x, double y, double width, double height, double tl, double tr, double br, double bl, bool isFilled)
        {
            if (tl > 0)
            {
                ctx.BeginFigure(new Point(x, y + tl), isFilled);
                ctx.LineTo(new Point(x + tl, y));
            }
            else
                ctx.BeginFigure(new Point(x, y), isFilled);
            
            //////ctx.TraverseGeometry(x, y, width, height, tl, tr, br, bl, true);


            if (tr > 0)
            {
                ctx.LineTo(new Point(x + (width - tr), y));
                ctx.LineTo(new Point(x + width, y + tr));
            }
            else
                ctx.LineTo(new Point(x + width, y));
            

            if (br > 0)
            {
                ctx.LineTo(new Point(x + width, y + (height - br)));
                ctx.LineTo(new Point(x + (width - br), y + height));
            }
            else
                ctx.LineTo(new Point(x + width, y + height));
            

            if (bl > 0)
            {
                ctx.LineTo(new Point(x + bl, y + height));
                ctx.LineTo(new Point(x, y + (height - bl)));
            }
            else
                ctx.LineTo(new Point(x, y + height));


            ctx.EndFigure(true);
        }

        public static void TraverseGeometry(this StreamGeometryContext ctx, double x, double y, double width, double height, double tl, double tr, double br, double bl, bool outer)
        {
            List<Action> steps = new List<Action>()
            {
                () => ctx.LineTo(new Point(x + tl, y)),
                () => ctx.LineTo(new Point(x + (width - tr), y)),
                () => ctx.LineTo(new Point(x + width, y + tr)),
                () => ctx.LineTo(new Point(x + width, y + (height - br))),
                () => ctx.LineTo(new Point(x + (width - br), y + height)),
                () => ctx.LineTo(new Point(x + bl, y + height)),
                () => ctx.LineTo(new Point(x, y + (height - bl))),
            };
            if (!outer)
                steps.Insert(0, () => new Point(x, y + tl));

            for (int i = 0; i < steps.Count; i++)
                steps[outer ? i : steps.Count - (i)]();
        }
        /*public static void CreateGeometry(this StreamGeometryContext ctx, double width, double height, double tl, double tr, double br, double bl, bool isFilled)
        {
            ctx.BeginFigure(new Point(1, tl), isFilled);
            ctx.LineTo(new Point(tl, 1));
            ctx.LineTo(new Point(width - tr, 1));
            ctx.LineTo(new Point(width, tr));
            ctx.LineTo(new Point(width, height - br));
            ctx.LineTo(new Point(width - br, height));
            ctx.LineTo(new Point(bl, height));
            ctx.LineTo(new Point(1, height - bl));
            ctx.EndFigure(true);
        }*/
    }
}