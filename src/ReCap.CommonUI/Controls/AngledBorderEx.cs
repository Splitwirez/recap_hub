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

namespace ReCap.CommonUI
{
    public enum InvertedSide
    {
        None = -1,
        Bottom = 1,
        Left = 0,
        Right = 2,
        Top = 3
    }

    public class AngledBorderEx : AngledBorderBase
    {
        /// <summary>
        /// Defines the <see cref="TopLeftCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TopLeftCutProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(TopLeftCut), 0);
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double TopLeftCut
        {
            get => GetValue(TopLeftCutProperty);
            set => SetValue(TopLeftCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="TopRightCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TopRightCutProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(TopRightCut), 0);
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double TopRightCut
        {
            get => GetValue(TopRightCutProperty);
            set => SetValue(TopRightCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomRightCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomRightCutProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(BottomRightCut), 0);
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double BottomRightCut
        {
            get => GetValue(BottomRightCutProperty);
            set => SetValue(BottomRightCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomLeftCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomLeftCutProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(BottomLeftCut), 0);
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double BottomLeftCut
        {
            get => GetValue(BottomLeftCutProperty);
            set => SetValue(BottomLeftCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="TopLeftInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TopLeftInsetProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(TopLeftInset), 0);
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double TopLeftInset
        {
            get => GetValue(TopLeftInsetProperty);
            set => SetValue(TopLeftInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="TopRightInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TopRightInsetProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(TopRightInset), 0);
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double TopRightInset
        {
            get => GetValue(TopRightInsetProperty);
            set => SetValue(TopRightInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomRightInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomRightInsetProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(BottomRightInset), 0);
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double BottomRightInset
        {
            get => GetValue(BottomRightInsetProperty);
            set => SetValue(BottomRightInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomLeftInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomLeftInsetProperty =
            AvaloniaProperty.Register<AngledBorderEx, double>(nameof(BottomLeftInset), 0);
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double BottomLeftInset
        {
            get => GetValue(BottomLeftInsetProperty);
            set => SetValue(BottomLeftInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="InvertSide"/> property.
        /// </summary>
        public static readonly StyledProperty<InvertedSide> InvertSideProperty =
            AvaloniaProperty.Register<AngledBorderEx, InvertedSide>(nameof(InvertSide), InvertedSide.None);
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public InvertedSide InvertSide
        {
            get => GetValue(InvertSideProperty);
            set => SetValue(InvertSideProperty, value);
        }


        static AngledBorderEx()
        {
            Action<AngledBorderEx, AvaloniaPropertyChangedEventArgs> changedHandler = (s, e) => s.UpdateGeometry();

            AvaloniaProperty[] props = 
            {
                TopLeftCutProperty,
                TopLeftInsetProperty,
                TopRightCutProperty,
                TopRightInsetProperty,
                BottomRightCutProperty,
                BottomRightInsetProperty,
                BottomLeftCutProperty,
                BottomLeftInsetProperty,
                InvertSideProperty
            };

            
            foreach (var prop in props)
            {
                prop.Changed.AddClassHandler<AngledBorderEx>(changedHandler);
            }

            AffectsRender<AngledBorderEx>(props);
        }

        protected override (Geometry, Geometry, RoundedRect) RefreshGeometry()
        {
            //Console.WriteLine($"Updating geometries...");
            double width = Math.Round(Bounds.Width);
            double height = Math.Round(Bounds.Height);

            double minDimen = Math.Min(width, height);
            
            double tl = 0;
            double tr = 0;
            double br = 0;
            double bl = 0;

            double tlInset = 0;
            double trInset = 0;
            double brInset = 0;
            double blInset = 0;
            

            tl = Math.Min(TopLeftCut, minDimen);
            tr = Math.Min(TopRightCut, minDimen);
            br = Math.Min(BottomRightCut, minDimen);
            bl = Math.Min(BottomLeftCut, minDimen);

            tlInset = Math.Min(TopLeftInset, width);
            trInset = Math.Min(TopRightInset, width);
            brInset = Math.Min(BottomRightInset, width);
            blInset = Math.Min(BottomLeftInset, width);
            /*tlInset = Math.Min(TopLeftInset, minDimen - tl);
            trInset = Math.Min(TopRightInset, minDimen - tr);
            brInset = Math.Min(BottomRightInset, minDimen - br);
            blInset = Math.Min(BottomLeftInset, minDimen - bl);*/
            
            /*tl = Math.Round(tl, 0);
            tr = Math.Round(tr, 0);
            br = Math.Round(br, 0);
            bl = Math.Round(bl, 0);*/
            
            double strokeThickness = StrokeThickness;
            bool hasStroke = strokeThickness > 0;
            //double strokeHalf = hasStroke ? (strokeThickness / 2) : 0;
            double borderBothSides = strokeThickness * 2;
            
            double fillWidth = width - borderBothSides;
            double fillHeight = height - borderBothSides;

            var rect = new Rect(strokeThickness, strokeThickness, fillWidth, fillHeight);
            Thickness outsetTh = default(Thickness);
            
            var fillGeometry = new StreamGeometry();
            using (var ctx = fillGeometry.Open())
            {
                ctx.CreateGeometry(
                    /*strokeThickness,
                    strokeThickness,
                    fillWidth,
                    fillHeight,*/
                    0,
                    0,
                    width,
                    height,

                    tl,
                    tr,
                    br,
                    bl,
                    
                    tlInset,
                    trInset,
                    brInset,
                    blInset,
                    
                    InvertSide,
                    strokeThickness, false,
                    true, out outsetTh);
            }

            double prX = rect.X + outsetTh.Left;
            double prY = rect.Y + outsetTh.Top;
            double prWidth = rect.Width + (outsetTh.Right - outsetTh.Left); //(outsetTh.Left + outsetTh.Right);
            double prHeight = rect.Height + (outsetTh.Bottom - outsetTh.Top); //(outsetTh.Top + outsetTh.Bottom);
            var preRoundedRect = 
                //rect.Deflate(outsetTh).Inflate(outsetTh)
                new Rect(prX, prY, prWidth, prHeight)
                    //.Deflate(25)
                //new Rect(rect.Left - Math.Min(0, outsetTh.Left), rect.Top - Math.Min(0, outsetTh.Top), rect.Right + Math.Min(0, outsetTh.Right), rect.Bottom + Math.Min(0, outsetTh.Bottom))
                //new Rect(rect.Left + outsetTh.Left, rect.Top + outsetTh.Top, rect.Width - (outsetTh.Left + outsetTh.Right), rect.Height - (outsetTh.Top + outsetTh.Bottom))
                //rect
            ;
            var roundedRect = new RoundedRect(
                preRoundedRect,
                0, 0, 0, 0 
                //tl, tr, br, bl
                );
            /*double realAverageBorderThickness = _averageBorderThickness;
            _averageBorderThickness = 4;
            borderBothSides = _averageBorderThickness * 2;
            fillWidth = width - borderBothSides;
            fillHeight = height - borderBothSides;*/

            Geometry strokeGeometry = null;
            if (strokeThickness > 0)
            {
                /*double diagonalDiff = Math.Round(strokeThickness / 2, MidpointRounding.AwayFromZero);
                double diagonalDiffSm = strokeThickness - diagonalDiff; //(int)diagonalDiff;
                //strokeThickness / 2;

                tl = (tl > 0) ? tl + diagonalDiff : 0;
                tr = (tr > 0) ? tr + diagonalDiff : 0;
                br = (br > 0) ? br + diagonalDiff : 0;
                bl = (bl > 0) ? bl + diagonalDiff : 0;

                tlInset = (tlInset > 0) ? tlInset + diagonalDiff : 0;
                trInset = (trInset > 0) ? trInset + diagonalDiff : 0;
                brInset = (brInset > 0) ? brInset + diagonalDiff : 0;
                blInset = (blInset > 0) ? blInset + diagonalDiff : 0;*/
                var strokeOuterGeometry = new StreamGeometry();
                using (var ctx = strokeOuterGeometry.Open())
                {
                    ctx.CreateGeometry(
                        0,
                        0,
                        width,
                        height,
                        
                        tl,
                        tr,
                        br,
                        bl,
                        
                        tlInset,
                        trInset,
                        brInset,
                        blInset,
                        
                        InvertSide,
                        strokeThickness, true,
                        true, out _);
                    //////ctx.BeginFigure(new Point(0 + 1, 0 + tl), true);
                    //////ctx.TraverseGeometry(0, 0, width, height, tl, tr, br, bl, true);
                    //ctx.TraverseGeometry(_averageBorderThickness, _averageBorderThickness, fillWidth, fillHeight, tl - _averageBorderThickness, tr - _averageBorderThickness, br - _averageBorderThickness, bl - _averageBorderThickness, false);
                    //////ctx.EndFigure(true);
                }

                //_averageBorderThickness = realAverageBorderThickness;
                //strokeGeometry = new CombinedGeometry(GeometryCombineMode.Xor, strokeOuterGeometry, fillGeometry/*, new TranslateTransform(0, 0)*/);
                strokeGeometry = strokeOuterGeometry;
            }
            
            return 
            (
                fillGeometry,
                strokeGeometry,
                roundedRect
            );
        }
    }

    public static partial class AngledCorners
    {
        public static void CreateGeometry(
            this StreamGeometryContext ctx,
            
            double xRaw, double yRaw, double widthRaw, double heightRaw,

            double tlRaw, double trRaw, double brRaw, double blRaw,
            double tlInsetRaw, double trInsetRaw, double brInsetRaw, double blInsetRaw,
            
            InvertedSide invSide,
            double strokeThicknessRaw, bool isForStroke,
            bool isFilled, out Thickness outset)
        {
            outset = new Thickness(0);
            bool noXYInvert = invSide == InvertedSide.None;
            double deltaFromStroke = 0;
            
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
            /*double strokeHalf = hasStroke
                ? strokeThickness / 2
                : 0;*/
            
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
                /*x += strokeHalf;
                y += strokeHalf;
                width -= strokeThickness;
                height -= strokeThickness;*/
                //double strokeThickness
                double invInsetDelta = strokeThickness * 1.41425;
                if (isInvLeft)
                {
                    tlInset = 0;
                    blInset = 0;
                    /*tl += strokeThickness;
                    bl += strokeThickness;
                    x += strokeThickness;
                    width -= strokeThickness;*/
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
                    /*if (Math.Min(tlInset, trInset) > 0)
                    {
                        tlInset -= invInsetDelta;
                        trInset -= invInsetDelta;
                    }
                    else
                    {
                        y += strokeThickness;
                        height -= strokeThickness;
                        
                        tl -= strokeThickness;
                        tr -= strokeThickness;
                    }*/
                }
                else if (isInvRight)
                {
                    trInset = 0;
                    brInset = 0;
                    //width -= strokeThickness;
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

                /*if (isInvTop || isInvLeft)
                    tlInset -= invInsetDelta;
                
                if (isInvTop || isInvRight)
                    trInset -= invInsetDelta;

                if (isInvBottom || isInvRight)
                    brInset -= invInsetDelta;

                if (isInvBottom || isInvLeft)
                    blInset -= invInsetDelta;*/
                /*if (isInvTop || isInvBottom)
                    if (isInvTop || isInvLeft)
                        tlInset -= invInsetDelta;
                    
                    if (isInvTop || isInvRight)
                        trInset -= invInsetDelta;

                    if (isInvBottom || isInvRight)
                        brInset -= invInsetDelta;

                    if (isInvBottom || isInvLeft)
                        blInset -= invInsetDelta;
                }
                else if (isInvLeft || isInvRight)
                {
                    if (isInvTop || isInvLeft)
                        tlInset -= invInsetDelta;
                    
                    if (isInvTop || isInvRight)
                        trInset -= invInsetDelta;

                    bool bottomNoInsets = (isInvBottom && (blInset <= 0) && (brInset <= 0)
                    if (isInvBottom || isInvRight)
                        brInset -= invInsetDelta;

                    if ( || isInvLeft)
                        blInset -= invInsetDelta;
                }*/

                /*tlInset += strokeThickness;
                trInset += strokeThickness;
                brInset += strokeThickness;
                blInset += strokeThickness;*/

                /*if (isInvLeft || isInvRight)
                {
                    width -= strokeThickness;

                    if (isInvLeft)
                        x += strokeThickness;
                }
                else if (isInvTop || isInvBottom)
                {
                    height -= strokeThickness;

                    if (isInvTop)
                        y += strokeThickness;
                }*/


                if (false)
                {
                double cutDeltaFromStroke = strokeThickness;
                double insetDeltaFromStroke = -strokeThickness;
                double cutDeltaFromStroke_Inset = -strokeThickness;


                if (isInvTop)
                {
                    tlInset += insetDeltaFromStroke;
                    trInset += insetDeltaFromStroke;
                    
                    /*tl += cutDeltaFromStroke_Inset;
                    tr += cutDeltaFromStroke_Inset;*/

                    //y += cutDeltaFromStroke;
                    //height -= cutDeltaFromStroke;
                    
                }
                else
                {
                    /*tl += cutDeltaFromStroke;
                    tr += cutDeltaFromStroke;*/
                    
                    //height -= cutDeltaFromStroke;
                }
                
                if (isInvBottom)
                {
                    brInset += insetDeltaFromStroke;
                    blInset += insetDeltaFromStroke;
                    
                    

                    /*br += cutDeltaFromStroke_Inset;
                    bl += cutDeltaFromStroke_Inset;*/

                    //height -= strokeThickness;
                }
                else
                {
                    /*br += cutDeltaFromStroke;
                    bl += cutDeltaFromStroke;*/
                }


                if (isInvLeft || isInvRight)
                {
                    width -= strokeThickness;
                    //width -= strokeThickness;

                    if (isInvLeft)
                    {
                        x += strokeThickness;
                    }
                }

                double leftMult = isInvLeft ? -1 : 1;
                double rightMult = isInvRight ? -1 : 1;

                if ((tl > 0))// && (!isInvLeft))
                    tl += strokeThickness * leftMult;
                
                if ((tr > 0))// && (!isInvRight))
                    tr += strokeThickness * rightMult;
                
                
                if ((br > 0))// && (!isInvRight))
                    br += strokeThickness * rightMult;
                
                if ((bl > 0))// && (!isInvLeft))
                    bl += strokeThickness * leftMult;
                
                /*double topCutDelta = strokeHalf; //isTop ? strokeHalf : strokeThickness;
                
                if ((tl > 0) && (!isInvLeft))
                    tl += topCutDelta;
                
                if ((tr > 0) && (!isInvRight))
                    tr += topCutDelta;
                
                
                double bottomCutDelta = strokeHalf; //isBottom ? strokeHalf : strokeThickness;
                
                if ((br > 0) && (!isInvRight))
                    br += bottomCutDelta;
                
                if ((bl > 0) && (!isInvLeft))
                    bl += bottomCutDelta;*/
                
                
                if (false)
                {
                double topDeltaFromStroke = deltaFromStroke;
                double bottomDeltaFromStroke = topDeltaFromStroke;

                double topInsetDeltaFromStroke = -deltaFromStroke;
                double bottomInsetDeltaFromStroke = topInsetDeltaFromStroke;

                //deltaFromStroke = -deltaFromStroke;
                
                if (!isForStroke)
                {
                    if (isInvTop)
                        topInsetDeltaFromStroke = -topInsetDeltaFromStroke;
                    
                    if (isInvBottom)
                        bottomInsetDeltaFromStroke = -bottomInsetDeltaFromStroke;

                    if (tlInset > 0)
                        tlInset += topInsetDeltaFromStroke;
                    
                    if (trInset > 0)
                        trInset += topInsetDeltaFromStroke;


                    if (brInset > 0)
                        brInset += bottomInsetDeltaFromStroke;
                    
                    if (blInset > 0)
                        blInset += bottomInsetDeltaFromStroke;
                    

                
                    /*if (isTop)
                        topDeltaFromStroke = -topDeltaFromStroke;
                    
                    if (isBottom)
                        bottomDeltaFromStroke = -bottomDeltaFromStroke;*/


                    
                    /*if (tl > 0)
                        tl += topDeltaFromStroke;
                    
                    if (tr > 0)
                        tr += topDeltaFromStroke;
                    
                    if (br > 0)
                        br += bottomDeltaFromStroke;
                    
                    if (bl > 0)
                        bl += bottomDeltaFromStroke;*/
                }
                
                /*else
                {
                    deltaFromStroke = strokeThickness / 2.0;

                    tl += deltaFromStroke;
                    tr += deltaFromStroke;
                    br += deltaFromStroke;
                    bl += deltaFromStroke;
                }*/
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
            
            /*if
            (
                   (tlInset == 0)
                && (trInset == 0)
                && (blInset == 0)
                && (brInset == 0)
                && (noXYInvert)
            )
            {
                CreateGeometry(ctx, x, y, width, height, tl, tr, br, bl, isFilled);
                return;
            }*/

            List<Point> points = new List<Point>();
            List<Point> newPoints = new List<Point>();

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
            
            bool returnToTopRight = true;

            //top left
            ResolveCorner(tl, tlInset, nearX, nearY, leftInvert, topInvert, ref points);

            //top right
            ResolveCorner(tr, trInset, farX, nearY, rightInvert, topInvert, ref newPoints);
            goto addNewPoints;
            topRight:

            //bottom right
            ResolveCorner(br, brInset, farX, farY, rightInvert, bottomInvert, ref points);

            //bottom left
            ResolveCorner(bl, blInset, nearX, farY, leftInvert, bottomInvert, ref newPoints);
            returnToTopRight = false;
            goto addNewPoints;
            bottomLeft:

            
            ctx.BeginFigure(points[0], isFilled);
            //foreach (var pnt in points)
            int pntCount = points.Count;
            for (int pntIndex = 1; pntIndex < pntCount; pntIndex++)
            {
                ctx.LineTo(points[pntIndex]);
            }

            ctx.EndFigure(true);


            outset = new Thickness(
                leftInvert ? leftOutset : -leftOutset
                , topInvert ? topOutset : -topOutset
                , rightInvert ? rightOutset : -rightOutset
                , bottomInvert ? bottomOutset : -bottomOutset);
            return;

            addNewPoints:
                //var newPointsR = newPoints.Reverse();
                /*int newPointCount = newPoints.Count - 1;
                for (int i = newPointCount; i > 0; i--)
                    points.Add(newPoints[i]);*/
                
                int insertIndex = points.Count;
                foreach (Point pnt in newPoints)
                    points.Insert(insertIndex, pnt);
                newPoints.Clear();

                if (returnToTopRight)
                    goto topRight;
                else
                    goto bottomLeft;
        }

        static void ResolveCorner(double cut, double inset, double startX, double startY, bool xInvert, bool yInvert, ref List<Point> points)
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
    }
}