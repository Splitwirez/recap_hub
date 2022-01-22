using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReCap.Hub.Controls
{
    public class SlicedImage : AvaloniaObject, IPresentableImage
    {
        /// <summary>
        /// Defines the <see cref="Source"/> property.
        /// </summary>
        public static readonly StyledProperty<IImage> SourceProperty =
            AvaloniaProperty.Register<SlicedImage, IImage>(nameof(Source));

        /// <summary>
        /// Defines the <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> BorderThicknessProperty =
            Border.BorderThicknessProperty.AddOwner<SlicedImage>();



        static SlicedImage()
        {
            Action<SlicedImage, AvaloniaPropertyChangedEventArgs> changed = (s, e) =>
            {
                s.AffectRender?.Invoke(s, e);
                s.AffectMeasure?.Invoke(s, e);
                s.OnImagePropsChanged();
            };

            foreach (AvaloniaProperty prop in new AvaloniaProperty[]
            {
                SourceProperty,
                BorderThicknessProperty
            })
            {
                prop.Changed.AddClassHandler(changed);
            }
        }

        public SlicedImage()
            : base()
        {
            OnImagePropsChanged();
        }

        IImage _src = null;
        double _srcWidth = 0;
        double _srcHeight = 0;


        double _left = 0;
        double _top = 0;
        double _right = 0;
        double _bottom = 0;
        
        Rect _tlSrc = new Rect();
        Rect _tcSrc = new Rect();
        Rect _trSrc = new Rect();
        
        Rect _mlSrc = new Rect();
        Rect _mcSrc = new Rect();
        Rect _mrSrc = new Rect();

        Rect _blSrc = new Rect();
        Rect _bcSrc = new Rect();
        Rect _brSrc = new Rect();

        bool _drawAnything = false;
        bool _drawBorders = false;

        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectMeasure;
        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectRender;

        void OnImagePropsChanged()
        {
            _drawAnything = true;

            _src = Source;
            Thickness bdt = BorderThickness;



            _drawAnything = _src != null;

            if (!_drawAnything)
                return;




            _srcWidth = _src.Size.Width;
            _srcHeight = _src.Size.Height;

            _drawAnything =
                   (_srcWidth > 0)
                && (_srcHeight > 0)
            ;

            if (!_drawAnything)
                return;

            _drawBorders = _drawAnything && (bdt != null);

            if (_drawBorders)
            {
                _left = Math.Max(bdt.Left, 0);
                _top = Math.Max(bdt.Top, 0);
                _right = Math.Max(bdt.Right, 0);
                _bottom = Math.Max(bdt.Bottom, 0);

                _drawBorders =
                       (_left > 0)
                    || (_top > 0)
                    || (_right > 0)
                    || (_bottom > 0)
                ;


                if (_drawBorders)
                {
                    double centerFar = _srcWidth - _right;
                    double middleFar = _srcHeight - _bottom;

                    double centerWidth = centerFar - _left;
                    double middleHeight = middleFar - _top; 
                    //double centerWidth = _srcWidth - (_left + _right);
                    //double middleheight = _srcHeight - (_top + _bottom);


                    _tlSrc = new Rect(0, 0, _left, _top);
                    _tcSrc = new Rect(_left, 0, centerWidth, _top);
                    _trSrc = new Rect(centerFar, 0, _right, _top);

                    _mlSrc = new Rect(0, _top, _left, middleHeight);
                    _mcSrc = new Rect(_left, _top, centerWidth, middleHeight);
                    _mrSrc = new Rect(centerFar, _top, _right, middleHeight);

                    _blSrc = new Rect(0, middleFar, _left, _bottom);
                    _bcSrc = new Rect(_left, middleFar, centerWidth, _bottom);
                    _brSrc = new Rect(centerFar, middleFar, _right, _bottom);
                }
            }
        }

        /// <summary>
        /// Gets or sets the image that will be displayed.
        /// </summary>
        [Content]
        public IImage Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Gets or sets a value controlling how the image will be stretched.
        /// </summary>
        public Thickness BorderThickness
        {
            get => GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public void Render(ImagePresenter presenter, DrawingContext context)
        {
            if (!_drawAnything)
                return;

            Size size = presenter.Bounds.Size;
            if (!_drawBorders)
            {
                context.DrawImage(_src,
                      new Rect(0, 0, _srcWidth, _srcHeight)
                    , new Rect(0, 0, size.Width, size.Height)
                    );
                return;
            }
            double destWidth = size.Width;
            double destHeight = size.Height;

            double centerFar = destWidth - _right;
            double middleFar = destHeight - _bottom;

            double centerWidth = centerFar - _left;
            double middleHeight = middleFar - _top;


            bool drawLeft = _left > 0;
            bool drawRight = _right > 0;


            if (_top > 0)
            {
                //top left
                if (drawLeft)
                {
                    context.DrawImage(_src, _tlSrc,
                        new Rect(0, 0, _left, _top)
                    );
                }

                //top center
                context.DrawImage(_src, _tcSrc,
                    new Rect(_left, 0, centerWidth, _top)
                );

                //top right
                if (drawRight)
                {
                    context.DrawImage(_src, _trSrc,
                        new Rect(centerFar, 0, _right, _top)
                    );
                }
            }

            
            //middle left
            if (drawLeft)
            {
                context.DrawImage(_src, _mlSrc,
                    new Rect(0, _top, _left, middleHeight)
                );
            }

            //middle center
            context.DrawImage(_src, _mcSrc,
                new Rect(_left, _top, centerWidth, middleHeight)
            );

            //middle right
            if (drawRight)
            {
                context.DrawImage(_src, _mrSrc,
                    new Rect(centerFar, _top, _right, middleHeight)
                );
            }


            if (_bottom > 0)
            {
                //bottom left
                if (drawLeft)
                {
                    context.DrawImage(_src, _blSrc,
                        new Rect(0, middleFar, _left, _bottom)
                    );
                }

                //top center
                context.DrawImage(_src, _bcSrc,
                    new Rect(_left, middleFar, centerWidth, _bottom)
                );

                //top right
                if (drawRight)
                {
                    context.DrawImage(_src, _brSrc,
                        new Rect(centerFar, middleFar, _right, _bottom)
                    );
                }
            }
        }

        public Size MeasureOverride(ImagePresenter presenter, Size availableSize, Size baseSize)
        {
            if (!_drawAnything)
                return baseSize;

            return new Size(
                  Math.Max(baseSize.Width, _left + _right)
                , Math.Max(baseSize.Height, _top + _bottom)
            );
        }
    }
}
