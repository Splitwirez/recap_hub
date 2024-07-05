using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
namespace ReCap.CommonUI
{
    public partial class TitleBarAngledBorder
        : AngledBorderBase
    {
        static StreamGeometryContext CreateGeometry(
                ref StreamGeometry streamGeom,

                double xRaw, double yRaw, double widthRaw, double heightRaw,

                double tlRaw, double trRaw, double brRaw, double blRaw,
                double brInsetRaw, double blInsetRaw,

                bool cbOnLeft,
                double cb,
                double cbInset,

                double strokeThicknessRaw, bool isForStroke, double strokeBottomCutOff,
                out Thickness outset)
        {
            StreamGeometryContext ctx = streamGeom.Open();

            double x = xRaw;
            double y = yRaw;
            double width = widthRaw;
            double height = heightRaw;

            double tl = tlRaw;
            double tr = trRaw;
            double br = brRaw;
            double bl = blRaw;

            double brInset = brInsetRaw;
            double blInset = blInsetRaw;

            double strokeThickness = strokeThicknessRaw;
            double strokeBothSides = strokeThickness * 2;
            bool hasStroke = strokeThicknessRaw > 0;

            bool hasStrokeBottomCutOff = isForStroke && (strokeBottomCutOff > 0);


            if (hasStroke && !isForStroke)
            {
                x += strokeThickness;
                y += strokeThickness;
                width -= strokeBothSides;
                height -= strokeBothSides;

                double invInsetDelta = strokeThickness * 1.41425;
                

                bool rNonZero = brInset > 0;
                bool lNonZero = blInset > 0;


                if (rNonZero)
                    brInset -= invInsetDelta;
                else
                    br -= strokeThickness;

                
                if (lNonZero)
                    blInset -= invInsetDelta;
                else
                    bl -= strokeThickness;

                
                if (!(rNonZero || lNonZero))
                    height -= strokeThickness;
            }

            if (tlRaw <= 0)
                tl = 0;

            if (trRaw <= 0)
                tr = 0;

            if (brRaw <= 0)
                br = 0;

            if (blRaw <= 0)
                bl = 0;


            double bottomOutset = Math.Max(bl, br);

            double nearX = x;
            double nearY = y;
            double farX = x + width;
            double farY = (y + height) - bottomOutset;
            double strokeBottomCutOffFarY = farY;


            Point outerPoint;
            
            bool hasInsetPoint;
            Point insetPoint;
            
            bool hasCutPoint;
            Point cutPoint;

            //top left
            AngledBorderUtils.ResolveCorner(tl, 0, nearX, nearY, false, false
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
            if (hasStrokeBottomCutOff)
            {
                strokeBottomCutOffFarY -= strokeBottomCutOff;
                ctx.BeginFigure(new Point(nearX, strokeBottomCutOffFarY), true);
                ctx.LineTo(outerPoint);
            }
            else
            {
                ctx.BeginFigure(outerPoint, true);
            }
            if (hasInsetPoint)
                ctx.LineTo(insetPoint);
            if (hasCutPoint)
                ctx.LineTo(cutPoint);
            /*
            if (false && cbOnLeft)
            {
                AngledBorderUtils.ResolveCorner(tl, 0, nearX, nearY + cb, false, false, ref ctx, true);
                AngledBorderUtils.ResolveCorner(cb, cbInset, nearX, nearY, false, false, ref ctx, false);
            }
            else
            {
                AngledBorderUtils.ResolveCorner(tl, 0, nearX, nearY, false, false, ref ctx, true);
            }
            */


            //top right
            if (cbOnLeft)
            {
                AngledBorderUtils.ResolveCorner(tr, 0, farX, nearY, true, false
                    , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
                if (hasCutPoint)
                    ctx.LineTo(cutPoint);
                if (hasInsetPoint)
                    ctx.LineTo(insetPoint);
                ctx.LineTo(outerPoint);
            }
            else
            {
                AngledBorderUtils.ResolveCorner(cb, cbInset, farX - tr, nearY, true, false
                    , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
                if (hasCutPoint)
                    ctx.LineTo(cutPoint);
                if (hasInsetPoint)
                    ctx.LineTo(insetPoint);
                ctx.LineTo(outerPoint);

                AngledBorderUtils.ResolveCorner(tr, 0, farX, nearY + cb, true, false
                    , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
                if (hasCutPoint)
                    ctx.LineTo(cutPoint);
                if (hasInsetPoint)
                    ctx.LineTo(insetPoint);
                ctx.LineTo(outerPoint);
            }

            if (hasStrokeBottomCutOff)
            {
                ctx.LineTo(new Point(farX, strokeBottomCutOffFarY));
                ctx.EndFigure(true);
            }
            else
            {
                //bottom right
                AngledBorderUtils.ResolveCorner(br, brInset, farX, farY, true, true
                    , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
                ctx.LineTo(outerPoint);
                if (hasInsetPoint)
                    ctx.LineTo(insetPoint);
                if (hasCutPoint)
                    ctx.LineTo(cutPoint);


                //bottom left
                AngledBorderUtils.ResolveCorner(bl, blInset, nearX, farY, true, true
                    , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
                if (hasCutPoint)
                    ctx.LineTo(cutPoint);
                if (hasInsetPoint)
                    ctx.LineTo(insetPoint);
                ctx.LineTo(outerPoint);


                ctx.EndFigure(true);
            }

            outset = new Thickness(
                0, 0, 0
                , bottomOutset
            );

            return ctx;
        }
    }
}