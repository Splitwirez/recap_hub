using System;
using Avalonia;
using Avalonia.Media;
namespace ReCap.CommonUI
{
    public partial class TitleBarAngledBorder
        : AngledBorderBase
    {
        /// <summary>
        /// Defines the <see cref="TopLeftCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TopLeftCutProperty =
            AngledBorderEx.TopLeftCutProperty.AddOwner<TitleBarAngledBorder>();
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
            AngledBorderEx.TopRightCutProperty.AddOwner<TitleBarAngledBorder>();
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
            AngledBorderEx.BottomRightCutProperty.AddOwner<TitleBarAngledBorder>();
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
            AngledBorderEx.BottomLeftCutProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double BottomLeftCut
        {
            get => GetValue(BottomLeftCutProperty);
            set => SetValue(BottomLeftCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomRightInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomRightInsetProperty =
            AngledBorderEx.BottomRightInsetProperty.AddOwner<TitleBarAngledBorder>();
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
            AngledBorderEx.BottomLeftInsetProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double BottomLeftInset
        {
            get => GetValue(BottomLeftInsetProperty);
            set => SetValue(BottomLeftInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="CaptionButtonsExtent"/> property.
        /// </summary>
        public static readonly StyledProperty<double> CaptionButtonsExtentProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, double>(nameof(CaptionButtonsExtent), 0);
        public double CaptionButtonsExtent
        {
            get => GetValue(CaptionButtonsExtentProperty);
            set => SetValue(CaptionButtonsExtentProperty, value);
        }


        /*
        /// <summary>
        /// Defines the <see cref="IsMaximized"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsMaximizedProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, bool>(nameof(IsMaximized), false);
        public bool IsMaximized
        {
            get => GetValue(IsMaximizedProperty);
            set => SetValue(IsMaximizedProperty, value);
        }
        */


        /// <summary>
        /// Defines the <see cref="LeftSideCaptionButtons"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> LeftSideCaptionButtonsProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, bool>(nameof(LeftSideCaptionButtons), false);
        public bool LeftSideCaptionButtons
        {
            get => GetValue(LeftSideCaptionButtonsProperty);
            set => SetValue(LeftSideCaptionButtonsProperty, value);
        }




        static TitleBarAngledBorder()
        {
            AvaloniaProperty[] props =
            {
                CaptionButtonsExtentProperty,
                //IsMaximizedProperty,
                LeftSideCaptionButtonsProperty,

                BottomRightCutProperty,
                BottomLeftCutProperty,
                BottomRightInsetProperty,
                BottomLeftInsetProperty,
            };


            AffectsGeometry<TitleBarAngledBorder>(props);
            AffectsRender<TitleBarAngledBorder>(props);
        }

        protected override void RefreshGeometry(out Geometry fillGeometry, out Geometry strokeGeometry, out RoundedRect glowRect)
        {
            //Console.WriteLine($"Updating geometries...");
            double width = Math.Round(Bounds.Width);
            double height = Math.Round(Bounds.Height);

            double minDimen = Math.Min(width, height);

            double captionButtonsExtent = CaptionButtonsExtent;
            //bool isMaximized = IsMaximized;
            bool leftCaptionBtns = LeftSideCaptionButtons;

            double tl = Math.Min(TopLeftCut, minDimen);
            double tr = Math.Min(TopRightCut, minDimen);
            double br = Math.Min(BottomRightCut, minDimen);
            double bl = Math.Min(BottomLeftCut, minDimen);

            double tlInset = leftCaptionBtns
                ? captionButtonsExtent
                : 0
            ;
            double trInset = leftCaptionBtns
                ? 0
                : captionButtonsExtent
            ;
            double brInset = Math.Min(BottomRightInset, width);
            double blInset = Math.Min(BottomLeftInset, width);

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

            double captionButtons = 34;

            var fillGeom = new StreamGeometry();
            StreamGeometryContext ctx;
            using (ctx = CreateGeometry(
                    ref fillGeom,
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
                    /*
                    tlInset,
                    trInset,
                    */
                    brInset,
                    blInset,

                    leftCaptionBtns,
                    captionButtons,
                    captionButtonsExtent,

                    strokeThickness, false, -1,
                    out outsetTh)
            )
            {
                fillGeometry = fillGeom;

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

                strokeGeometry = null;
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
                    using (ctx = CreateGeometry(
                        ref strokeOuterGeometry,
                        0,
                        0,
                        width,
                        height,

                        tl,
                        tr,
                        br,
                        bl,

                        /*
                        tlInset,
                        trInset,
                        */
                        brInset,
                        blInset,

                        leftCaptionBtns,
                        captionButtons,
                        captionButtonsExtent,

                        strokeThickness, true, 0, //20,
                        out _))
                    {
                        //_averageBorderThickness = realAverageBorderThickness;
                        //strokeGeometry = new CombinedGeometry(GeometryCombineMode.Xor, strokeOuterGeometry, fillGeometry/*, new TranslateTransform(0, 0)*/);
                        strokeGeometry = strokeOuterGeometry;
                    }
                }

                glowRect = roundedRect;
            }
        }
    }
}