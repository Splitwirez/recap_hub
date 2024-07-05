using System;
using System.Collections.Generic;
using Avalonia;

namespace ReCap.CommonUI
{
    internal enum BoundsPortion : int
    {
        Left = 0b0001,
        Bottom = 0b0010,
        Right = 0b0100,
        Top = 0b1000,

        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomRight = Bottom | Right,
        BottomLeft = Bottom | Left,
    }
    internal static partial class AngledBorderUtils
    {
        static void AddInOrder(IEnumerable<Point> addFrom, ref List<Point> points)
        {
            foreach (var pt in addFrom)
            {
                points.Add(pt);
            }
        }

        static void AddReversed(IList<Point> addFrom, ref List<Point> points)
        {
            int lastPointIndex = addFrom.Count - 1;
            for (int i = lastPointIndex; i >= 0; i--)
            {
                points.Add(addFrom[i]);
            }
        }




        public static void ResolveCorner(double cut, double inset, double startX, double startY, bool xInvert, bool yInvert, ref List<Point> points)
        {
            if (cut > 0)
            {
                int xMult = xInvert ? -1 : 1;
                int yMult = yInvert ? -1 : 1;

                double xInner = cut * xMult;
                double yInner = cut * yMult;
                double xInset = inset * xMult;

                double x = startX;
                double y = startY;
                y += yInner;

                points.Add(new Point(x, y));
                if (inset > 0)
                {
                    x += xInset;
                    points.Add(new Point(x, y));
                }
                /*else if (inset < 0)
                {

                }*/
                /*else
                {*/
                //points.Add(new Point(x, startX + xInner));
                //points.Add(new Point(startY + yInner, y));
                x += xInner;
                y -= yInner;
                points.Add(new Point(x, y));
                //}
            }
            else
                points.Add(new Point(startX, startY));
        }

        /*public static void TraverseGeometry(this StreamGeometryContext ctx, double x, double y, double width, double height, double tl, double tr, double br, double bl, double tlInset, double trInset, double brInset, double blInset, bool outer)
        {
            if
            (
                    (tlInset == 0)
                && (trInset == 0)
                && (blInset == 0)
                && (brInset == 0)
            )
            {
                CreateGeometry(ctx, x, y, width, height, tl, tr, br, bl, outer);
                return;
            }


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
        }*/
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



        public static void ResolveCorner(double cut, double inset, double startX, double startY, BoundsPortion corner, bool verticalInvert
            , out Point outerPoint, out bool hasInsetPoint, out Point insetPoint, out bool hasCutPoint, out Point cutPoint)
        {
            bool xInvert = corner.HasFlag(BoundsPortion.Right);
            bool yInvert = corner.HasFlag(BoundsPortion.Bottom);

            ResolveCorner(cut, inset, startX, startY, xInvert
                , verticalInvert
                    ? !yInvert
                    : yInvert
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint)
            ;
        }
        public static void ResolveCorner(double cut, double inset, double startX, double startY, bool xInvert, bool yInvert
            , out Point outerPoint, out bool hasInsetPoint, out Point insetPoint, out bool hasCutPoint, out Point cutPoint)
        {
            if (cut <= 0)
            {
                outerPoint = new Point(startX, startY);
                hasInsetPoint = false;
                insetPoint = default;
                hasCutPoint = false;
                cutPoint = default;
                return;
            }
            
            int xMult = xInvert ? -1 : 1;
            int yMult = yInvert ? -1 : 1;

            double xInner = cut * xMult;
            double yInner = cut * yMult;
            double xInset = inset * xMult;

            double x = startX;
            double y = startY;
            y += yInner;

            outerPoint = new Point(x, y);
            if (inset > 0)
            {
                x += xInset;
                insetPoint = new Point(x, y);
                hasInsetPoint = true;
            }
            else
            {
                insetPoint = default;
                hasInsetPoint = false;
            }

            x += xInner;
            y -= yInner;
            cutPoint = new Point(x, y);
            hasCutPoint = true;
        }
    }
}
