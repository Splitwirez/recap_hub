using System;
using System.Globalization;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Reactive;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ReCap.CommonUI
{
    [PseudoClasses(_PSEUD_FILLS_SCREEN)]
    public class TitleBarUnderlay
        : TemplatedControl
    {
        const string _PSEUD_FILLS_SCREEN = ":fills_screen";

        /// <summary>
        /// Defines the <see cref="TitleBarHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TitleBarHeightProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, double>(nameof(TitleBarHeight), double.NaN);

        /// <summary>
        /// Represents the Height of the containing Window's TitleBar.
        /// </summary>
        public double TitleBarHeight
        {
            get => GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="DefaultTitleBarHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> DefaultTitleBarHeightProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, double>(nameof(DefaultTitleBarHeight), 30);

        /// <summary>
        /// Gets or sets the default Height of the element.
        /// </summary>
        public double DefaultTitleBarHeight
        {
            get => GetValue(DefaultTitleBarHeightProperty);
            set => SetValue(DefaultTitleBarHeightProperty, value);
        }
        

        /// <summary>
        /// Defines the <see cref="IsTitleBarHeightValid"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsTitleBarHeightValidProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsTitleBarHeightValid), false);

        /// <summary>
        /// Gets or sets the IsTitleBarHeightValid of the element.
        /// </summary>
        public bool IsTitleBarHeightValid
        {
            get => GetValue(IsTitleBarHeightValidProperty);
            set => SetValue(IsTitleBarHeightValidProperty, value);
        }
        

        /// <summary>
        /// Defines the <see cref="IsTitleBarVisible"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsTitleBarVisible), false);

        /// <summary>
        /// Gets or sets the IsTitleBarVisible of the element.
        /// </summary>
        public bool IsTitleBarVisible
        {
            get => GetValue(IsTitleBarVisibleProperty);
            set => SetValue(IsTitleBarVisibleProperty, value);
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (TopLevel.GetTopLevel(this) is Window window)
                window.BeginMoveDrag(e);
        }


        /// <summary>
        /// Defines the <see cref="IsWindowActive"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsWindowActiveProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsWindowActive), false);

        /// <summary>
        /// Gets or sets the IsWindowActive of the element.
        /// </summary>
        public bool IsWindowActive
        {
            get => GetValue(IsWindowActiveProperty);
            set => SetValue(IsWindowActiveProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="IsWindowMaximizedOrFullScreen"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsWindowMaximizedOrFullScreenProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsWindowMaximizedOrFullScreen), false);

        /// <summary>
        /// Gets or sets the IsWindowMaximizedOrFullScreen of the element.
        /// </summary>
        public bool IsWindowMaximizedOrFullScreen
        {
            get => GetValue(IsWindowMaximizedOrFullScreenProperty);
            set => SetValue(IsWindowMaximizedOrFullScreenProperty, value);
        }




        Window _window = null;
        TitleBar _titleBar = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (!(e.Root is Window window))
                return;
            
            _window = window;
            //this[!HeightProperty] = window[!Window.ExtendClientAreaTitleBarHeightHintProperty];
            //this[!IsVisibleProperty] = window[!Window.IsExtendedIntoWindowDecorationsProperty];

            RefreshIsWindowMaximizedOrFullScreen(_window.WindowState);
            _window.PropertyChanged += Window_PropertyChanged;
            this[!TitleBarHeightProperty] = _window[!Window.ExtendClientAreaTitleBarHeightHintProperty];
            this[!IsWindowActiveProperty] = _window[!Window.IsActiveProperty];

            var chromeOverlay = ChromeOverlayLayer.GetOverlayLayer(_window.Presenter);
            if (chromeOverlay == null)
                return;
            
            var chromeChildren = chromeOverlay.Children;
            foreach (var chromeChild in chromeChildren)
            {
                if (chromeChild is TitleBar titleBar)
                {
                    _titleBar = titleBar;
                    //_titleBar.PropertyChanged += TitleBar_PropertyChanged;
                    this[!IsTitleBarVisibleProperty] = _titleBar[!TitleBar.IsVisibleProperty];
                    break;
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            
            if (_window != null)
            {
                _window.PropertyChanged -= Window_PropertyChanged;
                IsWindowMaximizedOrFullScreen = false;
                _window = null;
            }

            if (_titleBar != null)
            {
                //_titleBar.PropertyChanged -= TitleBar_PropertyChanged;
                _titleBar = null;
            }
        }
        
        void Window_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Window.WindowStateProperty)
                RefreshIsWindowMaximizedOrFullScreen(e.GetNewValue<WindowState>());
        }

        void RefreshIsWindowMaximizedOrFullScreen(WindowState winState)
            => IsWindowMaximizedOrFullScreen =
                (winState == WindowState.Maximized)
                ||
                (winState == WindowState.FullScreen)
            ;
        /*
        void TitleBar_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TitleBar.IsVisibleProperty)
            {
                var isVisible = e.GetNewValue<bool>();
            }
        }

        void ValidateHeightProperties(double titleBarHeight, bool isTitleBarVisible)
        {

        }
        */




        static TitleBarUnderlay()
        {
            Extensions.MakeControlTypeNonInteractive<TitleBarUnderlay>();

            AffectsMeasure<TitleBarUnderlay>(TitleBarHeightProperty);

            TitleBarHeightProperty.Changed.AddClassHandler<TitleBarUnderlay>(TitleBarHeightProperty_Changed);
            IsWindowMaximizedOrFullScreenProperty.Changed.AddClassHandler<TitleBarUnderlay>(IsWindowMaximizedOrFullScreenProperty_Changed);
        }

        static void IsWindowMaximizedOrFullScreenProperty_Changed(TitleBarUnderlay underlay, AvaloniaPropertyChangedEventArgs args)
        {
            underlay.PseudoClasses.Set(_PSEUD_FILLS_SCREEN, args.GetNewValue<bool>());
        }

        static void TitleBarHeightProperty_Changed(TitleBarUnderlay underlay, AvaloniaPropertyChangedEventArgs args)
        {
            var newHeight = args.GetNewValue<double>();
            underlay.IsTitleBarHeightValid = newHeight >= 0;
        }

        protected override Type StyleKeyOverride => typeof(TitleBarUnderlay);
        static void WrLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
