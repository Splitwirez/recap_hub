using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace ReCap.CommonUI
{
    public partial class AngledBorderEx
        : AngledBorderBase
    {
        static StreamGeometryContext CreateGeometry(
                ref StreamGeometry streamGeom,

                double xRaw, double yRaw, double widthRaw, double heightRaw,

                double tlRaw, double trRaw, double brRaw, double blRaw,
                double tlInsetRaw, double trInsetRaw, double brInsetRaw, double blInsetRaw,

                InvertedSide invSide,
                double strokeThicknessRaw, bool isForStroke,
                out Thickness outset)
        {
            StreamGeometryContext ctx = streamGeom.Open();

            bool noXYInvert = invSide == InvertedSide.None;

            double x = xRaw;
            double y = yRaw;
            double width = widthRaw;
            double height = heightRaw;

            double tl = tlRaw;
            double tr = trRaw;
            double br = brRaw;
            double bl = blRaw;

            double tlInset = tlInsetRaw;
            double trInset = trInsetRaw;
            double brInset = brInsetRaw;
            double blInset = blInsetRaw;

            double strokeThickness = strokeThicknessRaw;
            double strokeBothSides = strokeThickness * 2;
            bool hasStroke = strokeThicknessRaw > 0;

            bool isInvLeft = invSide == InvertedSide.Left;
            bool isInvTop = invSide == InvertedSide.Top;
            bool isInvRight = invSide == InvertedSide.Right;
            bool isInvBottom = invSide == InvertedSide.Bottom;

            if (hasStroke && (!isForStroke))
            {
                x += strokeThickness;
                y += strokeThickness;
                width -= strokeBothSides;
                height -= strokeBothSides;

                double invInsetDelta = strokeThickness * 1.41425;
                if (isInvLeft)
                {
                    tlInset = 0;
                    blInset = 0;

                    x += strokeThickness;
                    width -= strokeThickness;

                    tl -= strokeThickness;
                    bl -= strokeThickness;
                }
                else if (isInvTop)
                {
                    bool lNonZero = tlInset > 0;
                    bool rNonZero = trInset > 0;

                    if (lNonZero)
                        tlInset -= invInsetDelta;
                    else
                        tl -= strokeThickness;

                    if (rNonZero)
                        trInset -= invInsetDelta;
                    else
                        tr -= strokeThickness;

                    if (!(lNonZero || rNonZero))
                    {
                        y += strokeThickness;
                        height -= strokeThickness;
                    }
                }
                else if (isInvRight)
                {
                    trInset = 0;
                    brInset = 0;

                    width -= strokeThickness;

                    tr -= strokeThickness;
                    br -= strokeThickness;
                }
                else if (isInvBottom)
                {
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
                    {
                        height -= strokeThickness;
                    }
                }
            }

            if (tlRaw <= 0)
                tl = 0;

            if (trRaw <= 0)
                tr = 0;

            if (brRaw <= 0)
                br = 0;

            if (blRaw <= 0)
                bl = 0;

            double leftOutset = 0;
            bool leftInvert = false;

            double topOutset = 0;
            bool topInvert = false;

            double rightOutset = 0;
            bool rightInvert = false;

            double bottomOutset = 0;
            bool bottomInvert = false;

            if (!noXYInvert)
            {
                if (isInvRight)
                {
                    rightInvert = true;
                    rightOutset = Math.Max(tr, br);
                    trInset = 0;
                    brInset = 0;
                }
                else if (isInvTop)
                {
                    topInvert = true;
                    topOutset = Math.Max(tl, tr);
                }
                else if (isInvBottom)
                {
                    bottomInvert = true;
                    bottomOutset = Math.Max(bl, br);
                }
                else if (isInvLeft)
                {
                    leftInvert = true;
                    leftOutset = Math.Max(tl, bl);
                    tlInset = 0;
                    blInset = 0;
                }
            }

            double nearX = x + leftOutset;
            double nearY = y + topOutset;
            double farX = (x + width) - rightOutset;
            double farY = (y + height) - bottomOutset;

            rightInvert = !rightInvert;
            bottomInvert = !bottomInvert;


            Point outerPoint;
            
            bool hasInsetPoint;
            Point insetPoint;
            
            bool hasCutPoint;
            Point cutPoint;


            //top left
            AngledBorderUtils.ResolveCorner(tl, tlInset, nearX, nearY, leftInvert, topInvert
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
            ctx.BeginFigure(outerPoint, true);
            if (hasInsetPoint)
                ctx.LineTo(insetPoint);
            if (hasCutPoint)
                ctx.LineTo(cutPoint);


            //top right
            AngledBorderUtils.ResolveCorner(tr, trInset, farX, nearY, rightInvert, topInvert
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
            if (hasCutPoint)
                ctx.LineTo(cutPoint);
            if (hasInsetPoint)
                ctx.LineTo(insetPoint);
            ctx.LineTo(outerPoint);


            //bottom right
            AngledBorderUtils.ResolveCorner(br, brInset, farX, farY, rightInvert, bottomInvert
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
            ctx.LineTo(outerPoint);
            if (hasInsetPoint)
                ctx.LineTo(insetPoint);
            if (hasCutPoint)
                ctx.LineTo(cutPoint);


            //bottom left
            AngledBorderUtils.ResolveCorner(bl, blInset, nearX, farY, leftInvert, bottomInvert
                , out outerPoint, out hasInsetPoint, out insetPoint, out hasCutPoint, out cutPoint);
            if (hasCutPoint)
                ctx.LineTo(cutPoint);
            if (hasInsetPoint)
                ctx.LineTo(insetPoint);
            ctx.LineTo(outerPoint);


            ctx.EndFigure(true);

            outset = new Thickness(
                leftInvert ? leftOutset : -leftOutset
                , topInvert ? topOutset : -topOutset
                , rightInvert ? rightOutset : -rightOutset
                , bottomInvert ? bottomOutset : -bottomOutset);
            
            return ctx;
        }
    }
}