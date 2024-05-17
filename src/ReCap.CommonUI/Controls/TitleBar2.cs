using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input.Raw;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Reactive;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


using static ReCap.CommonUI.WinUnmanagedMethods;

namespace ReCap.CommonUI
{
    [TemplatePart("PART_CaptionButtons", typeof(CaptionButtons))]
    public partial class TitleBar2 : ContentControl
    {
        const string PSE_ACTIVE = ":active";
        const string PSE_FILL_SCREEN = ":fillScreen";

        static bool DefaultToLeftSideButtons => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly StyledProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.Register<TitleBar2, bool>(nameof(LeftSideButtons), defaultValue: DefaultToLeftSideButtons);
        
        public bool LeftSideButtons
        {
            get => GetValue(LeftSideButtonsProperty);
            set => SetValue(LeftSideButtonsProperty, value);
        }


        public static readonly DirectProperty<TitleBar2, ResizeEdge> ResizeEdgeProperty =
            AvaloniaProperty.RegisterDirect<TitleBar2, ResizeEdge>(nameof(ResizeEdge), s => s.ResizeEdge, (s, v) => s.ResizeEdge = v);

        private ResizeEdge _resizeEdge = null;
        public ResizeEdge ResizeEdge
        {
            get => _resizeEdge;
            set => SetAndRaise(ResizeEdgeProperty, ref _resizeEdge, value);
        }


        public static readonly DirectProperty<TitleBar2, CaptionButtons> CaptionButtonsProperty =
            AvaloniaProperty.RegisterDirect<TitleBar2, CaptionButtons>(nameof(CaptionButtons), s => s.CaptionButtons, (s, v) => s.CaptionButtons = v);

        private CaptionButtons _captionButtons = null;
        public CaptionButtons CaptionButtons
        {
            get => _captionButtons;
            set => SetAndRaise(CaptionButtonsProperty, ref _captionButtons, value);
        }


        static TitleBar2()
        {
            /*ResizeEdgeProperty.Changed.AddClassHandler<TitleBar2>((sender, e) =>
            {
                //if (e.OldValue != null)

                if ((e.NewValue != null) && (e.NewValue is ResizeEdge newEdge))
                {
                    newEdge.
                }
            });*/
            CaptionButtonsProperty.Changed.AddClassHandler<TitleBar2>((s, e) => s.OnCaptionButtonsPropertyChanged(e));
            LeftSideButtonsProperty.Changed.AddClassHandler<TitleBar2>((s, e) => s.OnLeftSideButtonsChanged(e));
        }

        void OnLeftSideButtonsChanged(AvaloniaPropertyChangedEventArgs e)
            => RefreshCaptionButtons(_captionButtons, _captionButtonsProxy);

        void OnCaptionButtonsPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            (var oldVal, var newVal) = e.GetOldAndNewValue<CaptionButtons>();
            if (oldVal != null)
            {
                oldVal.Detach();
                oldVal.TemplateApplied -= CaptionButtons_TemplateApplied;
            }
            RefreshCaptionButtons(newVal, _captionButtonsProxy);
        }


        private CaptionButtons _prevCaptionButtons = null;
        void RefreshCaptionButtons(CaptionButtons btn, Control captionButtonsProxy)
        {
            if (btn == null)
            {
                _prevCaptionButtons = null;
                return;
            }

            if (captionButtonsProxy == null)
                return;

            if (_prevCaptionButtons != btn)
            {
                btn.TemplateApplied += CaptionButtons_TemplateApplied;
                PrepCaptionButtonsTemplate();
                /*var maxButton = newVal.FindNameScope().Find<Button>("PART_RestoreButton");
            
                maxButton.IsVisible = true;*/

                if (VisualRoot is Window window)
                    btn.Attach(window);


                _prevCaptionButtons = btn;
            }

            captionButtonsProxy.Tag = btn;
            
            btn.HorizontalAlignment = LeftSideButtons
                ? HorizontalAlignment.Left
                : HorizontalAlignment.Right
            ;

            Debug.WriteLine(nameof(RefreshCaptionButtons));
        }


        Button _minimizeButton = null;
        Button _restoreButton = null;
        Button _closeButton = null;
        Control _captionButtonsProxy = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            /*e.NameScope.Find<Button>("PART_CloseButton").Click += (s, a) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.Close();
            };

            e.NameScope.Find<Button>("PART_MaximizeButton").Click += (s, a) => ToggleMaximize();

            e.NameScope.Find<Button>("PART_MinimizeButton").Click += (s, a) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.WindowState = WindowState.Minimized;
            };*/
            if (!(VisualRoot is Window window))
                return;

            var dragGrip = e.NameScope.Find<TemplatedControl>("PART_DragGrip");
            
            dragGrip.PointerPressed += (s, a) =>
            {
                if (a.ClickCount <= 1)
                {
                    if (VisualRoot is Window win)
                        win.BeginMoveDrag(a);
                }
            };

            dragGrip.DoubleTapped += (s, a) => ToggleMaximize();

            _captionButtonsProxy = e.NameScope.Find<Control>("PART_CaptionButtonsProxy");
            RefreshCaptionButtons(_captionButtons, _captionButtonsProxy);
            //var oldWinStateChanged = window.PlatformImpl.WindowStateChanged;
            window.SizeChanged += (s, a) =>
            {
                var state = window.WindowState;

                var edge = ResizeEdge;
                if (edge != null)
                    edge.IsVisible = !((state == WindowState.Maximized) || (state == WindowState.FullScreen));
            };
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is Window win)
            {
                Attach(win);
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            Detach();
        }

        Window _rootWindow = null;
        IDisposable _stateChangedHandler = null;
        void Attach(Window window)
        {
            if (window == null)
                return;
            
            _stateChangedHandler = window.GetPropertyChangedObservable(Window.WindowStateProperty).Subscribe(e => 
            {
                if (e.Sender is Window win)
                    FillsScreen(win);
            });

            window.Activated += RootWindow_Activated;
            window.Deactivated += RootWindow_Deactivated;
            
            _rootWindow = window;
            PseudoClasses.Set(PSE_ACTIVE, window.IsActive);
            FillsScreen(window);
        }

        void FillsScreen(Window win)
            => PseudoClasses.Set(PSE_FILL_SCREEN, (win.WindowState == WindowState.Maximized) || (win.WindowState == WindowState.FullScreen));

        void Detach()
        {
            if (_stateChangedHandler != null)
                _stateChangedHandler.Dispose();
            
            if (_rootWindow == null)
                return;
            
            _rootWindow.Activated -= RootWindow_Activated;
            _rootWindow.Deactivated -= RootWindow_Deactivated;

            _rootWindow = null;
        }

        void RootWindow_Activated(object sender, EventArgs e)
            => PseudoClasses.Set(PSE_ACTIVE, true);

        void RootWindow_Deactivated(object sender, EventArgs e)
            => PseudoClasses.Set(PSE_ACTIVE, false);

        

        private WndProcDelegate _wndProc;
        IntPtr _newProcPtr = IntPtr.Zero;
        IntPtr _oldWndProc = IntPtr.Zero;

        IPseudoClasses _minPseudo = null;
        IPseudoClasses _maxPseudo = null;
        IPseudoClasses _closePseudo = null;
        bool _prevMinPointerOver = false;
        bool _prevMaxPointerOver = false;
        bool _prevClosePointerOver = false;
        void CaptionButtons_TemplateApplied(object sender, TemplateAppliedEventArgs e)
        {
            Debug.WriteLine(nameof(CaptionButtons_TemplateApplied));

            _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");
            TryGetPseudoClasses(_minimizeButton, out _minPseudo);

            _restoreButton = e.NameScope.Find<Button>("PART_RestoreButton");
            TryGetPseudoClasses(_restoreButton, out _maxPseudo);

            _closeButton = e.NameScope.Find<Button>("PART_CloseButton");
            TryGetPseudoClasses(_closeButton, out _closePseudo);

            PrepCaptionButtons();
            (sender as CaptionButtons).TemplateApplied -= CaptionButtons_TemplateApplied;
        }

        void PrepCaptionButtonsTemplate()
        {
            var nameScope = _captionButtonsProxy.FindNameScope();
            var descendants = _captionButtonsProxy.GetVisualDescendants();
            
            foreach (var descendant in descendants)
            {
                if (!(descendant is Button btn))
                    continue;

                string btnName = btn.Name;
                if (btnName == "PART_MinimizeButton")
                    _minimizeButton = btn;
                else if (btnName == "PART_RestoreButton")
                    _restoreButton = btn;
                else if (btnName == "PART_CloseButton")
                    _closeButton = btn;
            }

            //_minimizeButton = nameScope.Find<Button>("PART_MinimizeButton");
            TryGetPseudoClasses(_minimizeButton, out _minPseudo);

            //_restoreButton = nameScope.Find<Button>("PART_RestoreButton");
            TryGetPseudoClasses(_restoreButton, out _maxPseudo);

            //_closeButton = nameScope.Find<Button>("PART_CloseButton");
            TryGetPseudoClasses(_closeButton, out _closePseudo);

            PrepCaptionButtons();
        }


        void PrepCaptionButtons()
        {
            Debug.WriteLine($"{nameof(PrepCaptionButtons)}()\n{_minimizeButton != null}, {_restoreButton != null}, {_closeButton != null}");
            /*_minimizeButton.PointerExited += (s, _) =>
            {
                _minPseudo.Set(PS_POINTER, false);
                _prevMinPointerOver = false;
            };

            _restoreButton.PointerExited += (s, _) =>
            {
                _maxPseudo.Set(PS_POINTER, false);
                _prevMaxPointerOver = false;
            };

            _closeButton.PointerExited += (s, _) =>
            {
                _closePseudo.Set(PS_POINTER, false);
                _prevClosePointerOver = false;
            };*/




            //https://gist.github.com/AlexanderBaggett/d1504da93727a1778e8b5b3453946fc1
            if (
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                &&
                (VisualRoot is Window win)
                &&
                win.TryGetHWnd(out IntPtr hWnd)
            ) //OperatingSystem.IsWindows())
            {
                //SetWindowLongPtr(hWnd, (int)WindowLongParam.GWL_WNDPROC, );
                _wndProc = CaptionButtonsWndProc; //new WndProc(CaptionButtonsWndProc);
                _newProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                _oldWndProc = SetWindowLongPtr(hWnd, (int)(WindowLongParam.GWL_WNDPROC), _newProcPtr);
                // = Marshal.GetDelegateForFunctionPointer<WndProcDelegate>(_oldWndProc);


                /*Action<Rect> winPrevWhenPainted = win.PlatformImpl.Paint;
                Action<Rect> winWhenPainted;
                winWhenPainted = aRect =>
                {
                    var rect = PixelRect.FromRectWithDpi(aRect, win.PlatformImpl.DesktopScaling);
                /*var winSizeQ = win.FrameSize;
                if (winSizeQ.HasValue)
                {
                    var winPos = win.Position;
                    var winSize = win.FrameSize.GetValueOrDefault().;* /
                    /*var winTl = win.PointToScreen(win.Bounds.TopLeft);
                    var winBr = win.PointToScreen(win.Bounds.BottomRight);* /
                    MoveWindow(hWnd,
                        //winTl.X, winTl.Y, winBr.X - winTl.X, winBr.Y - winTl.Y
                        rect.X, rect.Y, rect.Width, rect.Height - 1
                        //winPos.X, winPos.Y, winSize.Width, winSize.Height
                        , true
                    );
                    win.PlatformImpl.Paint = winPrevWhenPainted;
                //}
                };
                win.PlatformImpl.Paint = winWhenPainted;*/
                var gwlStyle =  (int)(WindowLongParam.GWL_STYLE);
                var winStyle = GetWindowLongPtr(hWnd, gwlStyle);
                winStyle |= WS_CAPTION;
                SetWindowLongPtr(hWnd, gwlStyle, new IntPtr(winStyle));
            }
        }

        public IntPtr CaptionButtonsWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            var root = VisualRoot;
            if ((root is Window win) && TryHandleWmNcMessages(win, hWnd, msg, ref wParam, ref lParam, out IntPtr wmNcRet))
                return wmNcRet;
            
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        static readonly Dictionary<uint, uint> NC_TO_CLIENT_MSG = new Dictionary<uint, uint>()
        {
            //{ WM_NCMOUSEMOVE, WM_MOUSEMOVE },
            { WM_NCLBUTTONDOWN, WM_LBUTTONDOWN },
            { WM_NCLBUTTONUP, WM_LBUTTONUP },
        };
        const string PS_POINTER = ":pointerover";
        //const string PS_PRESSED = ":pressed";

        static readonly Dictionary<ResizeEdgeCell, IntPtr> EDGE_CELL_TO_NCHT = new Dictionary<ResizeEdgeCell, IntPtr>()
        {
            {  ResizeEdgeCell.Left, HTLEFT },
            {  ResizeEdgeCell.TopLeft, HTTOPLEFT },
            {  ResizeEdgeCell.Top, HTTOP },
            {  ResizeEdgeCell.TopRight, HTTOPRIGHT },
            {  ResizeEdgeCell.Right, HTRIGHT },
            {  ResizeEdgeCell.BottomRight, HTBOTTOMRIGHT },
            {  ResizeEdgeCell.Bottom, HTBOTTOM },
            {  ResizeEdgeCell.BottomLeft, HTBOTTOMLEFT },
        };

        //IntPtr _prevNcMouseMoveWParam = IntPtr.Zero;
        bool TryHandleWmNcMessages(Window win, IntPtr hWnd, uint msg, ref IntPtr wParam, ref IntPtr lParam, out IntPtr ret)
        {
            ret = IntPtr.Zero;
            if (msg == WM_STYLECHANGING)
            {
                ret = IntPtr.Zero;
                return true;
            }
            else if (msg == WM_NCCALCSIZE)
            {
                if (wParam != IntPtr.Zero)
                {
                    ret = IntPtr.Zero;
                    return true;
                }
                
                if (wParam != IntPtr.Zero)
                {
                    Debug.WriteLine("WM_NCCALCSIZE, nonzero");
                    //var nccsParams = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(lParam);


                    //https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-nccalcsize_params
                    NCCALCSIZE_PARAMS nccsParams = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(lParam, typeof(NCCALCSIZE_PARAMS));
                    //nccsParams.rgrc[0] = new full-window bounds
                    //nccsParams.rgrc[1] = old full-window bounds
                    //nccsParams.rgrc[2] = old client-area bounds
                    


                    //nccsParams.rgrc[0] = new client-area bounds
                    //nccsParams.rgrc[1] = "valid destination rectangle" (what)
                    //nccsParams.rgrc[2] = "valid source rectangle" (what)
                    Marshal.StructureToPtr(nccsParams, lParam, false);
                    ret = WVR_REDRAW;
                    return true;
                }
                else
                {
                    Debug.WriteLine("WM_NCCALCSIZE, zero");
                    ret = IntPtr.Zero;
                    return true;
                }
            }
            else if (msg == WM_NCHITTEST)
            {
                //var lParamPosRaw = new PixelPoint(x, y);
                var lParamPos = new POINT(lParam);

                if (ScreenToClient(hWnd, ref lParamPos))
                {
                    /*var lParamPos = Avalonia.VisualExtensions.PointToClient(win, lParamPosRaw)
                    ;*/
                    //Console.WriteLine($"WM_NCHITTEST: {lParamPos.X}, {lParamPos.Y}");
                    //if (TryGetTransformedBounds(_restoreButton, out Rect restoreBounds) && restoreBounds.ContainsExclusive(lParamPos))

                    POINT point = new POINT(lParam);
                    if (TryGetHitEdge(point, out ResizeEdgeCell cell))
                    {
                        ret = EDGE_CELL_TO_NCHT[cell];
                        return true;
                    }
                    else if (TryGetHitButton(hWnd, lParamPos, out IntPtr htBtn, out Button btn))
                    {
                        //SendMessage(hWnd, )
                        //DefWindowProc(hWnd, msg, wParam, lParam)
                        ret = htBtn;
                        return true;
                    }
                    else
                    {
                        ret = HTCLIENT;
                        return true;
                    }
                    //VisualExtensions.PointToClient()
                    //VisualExtensions.
                }
                else
                {
                    //Console.WriteLine("ScreenToClient :b:roke");
                }
            }
            /*else if (msg == WM_MOUSEMOVE)
            {
                /*if
                (
                    (wParam != HTMINBUTTON)
                    && (wParam != HTMAXBUTTON)
                    && (wParam != HTCLOSE)
                )* /
                if (wParam == HTCLIENT)
                {
                    if (_prevMinPointerOver && (wParam != HTMINBUTTON))
                    {
                        _minPseudo.Set(PS_POINTER, false);
                        _prevMinPointerOver = false;
                    }

                    if (_prevMaxPointerOver && (wParam != HTMAXBUTTON))
                    {
                        _maxPseudo.Set(PS_POINTER, false);
                        _prevMaxPointerOver = false;
                    }

                    if (_prevClosePointerOver && (wParam != HTCLOSE))
                    {
                        _closePseudo.Set(PS_POINTER, false);
                        _prevClosePointerOver = false;
                    }
                }
            }*/
            else if (msg == WM_MOUSEMOVE)// && (wParam == HTCLIENT))
            {
                if (!IsCaptionButton(wParam))
                {
                    //UnHoverCaptionButtons();
                    //Debug.WriteLine($"WM_MOUSEMOVE CAPTIONBUTTON {wParam}");
                }
                /*else
                    Debug.WriteLine($"WM_MOUSEMOVE {wParam}");*/
            }
            else if (msg == WM_NCMOUSEMOVE)
            {
                //POINT point = new POINT(lParam);


                bool htMin = (wParam == HTMINBUTTON);
                bool htMax = (wParam == HTMAXBUTTON);
                bool htClose = (wParam == HTCLOSE);

                //Debug.WriteLine($"WM_NCMOUSEMOVE ({htMin}, {htMax}, {htClose})");
                _minPseudo.Set(PS_POINTER, htMin);
                _prevMinPointerOver = htMin;

                _maxPseudo.Set(PS_POINTER, htMax);
                _prevMaxPointerOver = htMax;

                _closePseudo.Set(PS_POINTER, htClose);
                _prevClosePointerOver = htClose;
                /*if (wParam == HTCLIENT)
                {
                    UnHoverCaptionButtons();
                    Debug.WriteLine("WM_NCMOUSEMOVE HTCLIENT");
                }*/

                if (IsCaptionButton(wParam))
                {
                    POINT point = new POINT(lParam);
                    if (ScreenToClient(hWnd, ref point))
                    {
                        SendMessage(hWnd, WM_MOUSEMOVE, wParam, point.ToWMParam());
                    }
                }

                /*if (htMin || htMax || htClose)
                {*/
                ret = IntPtr.Zero;
                return true;
                //}
#if NO
                    bool min = htMin;
                    bool max = htMax;
                    bool close = htClose;
                    if (min)
                    {
                        max = false;
                        close = false;
                    }
                    else if (max)
                    {
                        min = false;
                        close = false;
                    }
                    else if (close)
                    {
                        min = false;
                        max = false;
                    }
                    else
                    {
                        min = false;
                        max = false;
                        close = false;
                    }

                    _minPseudo.Set(PS_POINTER, min);
                    _prevMinPointerOver = min;

                    _maxPseudo.Set(PS_POINTER, max);
                    _prevMaxPointerOver = max;

                    _closePseudo.Set(PS_POINTER, close);
                    _prevClosePointerOver = close;

                    ret = IntPtr.Zero;
                    return true;
#endif

                /*else
                {
                    UnHoverCaptionButtons();
                    /*ret = IntPtr.Zero;
                    return true;* /
                }*/
            }
            //else if (htMin || htMax || htClose) //if ((msg != WM_NCMOUSEMOVE)) // && (max || min || close))
            else if
            (/*(
                (msg == WM_NCMOUSEMOVE)
                || (msg == WM_NCLBUTTONDOWN)
                || (msg == WM_NCLBUTTONUP)
            ) && */NC_TO_CLIENT_MSG.TryGetValue(msg, out uint clientMsg)
            )
            {
                if (IsCaptionButton(wParam))
                {
                    POINT point = new POINT(lParam);
                    if (ScreenToClient(hWnd, ref point))
                    {
                        SendMessage(hWnd, clientMsg, HTCLIENT, point.ToWMParam());
                        ret = IntPtr.Zero;
                        return true;
                    }
                }
            }

            return false;
        }


        bool IsCaptionButton(IntPtr wParam)
        {
            return
                (wParam == HTMINBUTTON)
                || (wParam == HTMAXBUTTON)
                || (wParam == HTCLOSE)
            ;
        }

        bool TryGetHitEdge(POINT point, out ResizeEdgeCell cell)
        {
            cell = ResizeEdgeCell.None;
            var edge = ResizeEdge;
            if (edge == null)
            {
                //Debug.WriteLine("NULL EDGE");
                return false;
            }
            PixelPoint avScreenPoint = new PixelPoint(point.X, point.Y);
            var avClientPnt = edge.PointToClient(avScreenPoint);
            cell = edge.HitTestEx(avClientPnt.X, avClientPnt.Y);
            return (cell != ResizeEdgeCell.None);
        }

        /*void SetCaptionButtonPseudoClasses(string pseudoName, bool min, bool max, bool close)
        {
            if (TryGetPseudoClasses(_minimizeButton, out IPseudoClasses minPseudo))
            {
                minPseudo.Set(pseudoName, min);
            }

            if (TryGetPseudoClasses(_restoreButton, out IPseudoClasses maxPseudo))
            {
                maxPseudo.Set(pseudoName, max);
            }

            if (TryGetPseudoClasses(_closeButton, out IPseudoClasses closePseudo))
            {
                closePseudo.Set(pseudoName, close);
            }
        }*/


        void UnHoverCaptionButtons()
        {
            if (_prevMinPointerOver)
            {
                _minPseudo.Set(PS_POINTER, false);
                _prevMinPointerOver = false;
            }

            if (_prevMaxPointerOver)
            {
                _maxPseudo.Set(PS_POINTER, false);
                _prevMaxPointerOver = false;
            }

            if (_prevClosePointerOver)
            {
                _closePseudo.Set(PS_POINTER, false);
                _prevClosePointerOver = false;
            }
        }

        /*bool TryGetCaptionButtonPseudoClasses(string pseudoName, out bool min, out bool max, out bool close)
        {
            min = false;
            max = false;
            close = false;
            int retVal = 0;
            if (TryGetPseudoClasses(_minimizeButton, out IPseudoClasses minPseudo))
            {
                min = minPseudo.Contains(pseudoName);
                retVal++;
            }

            if (TryGetPseudoClasses(_restoreButton, out IPseudoClasses maxPseudo))
            {
                max = maxPseudo.Contains(pseudoName);
                retVal++;
            }

            if (TryGetPseudoClasses(_closeButton, out IPseudoClasses closePseudo))
            {
                close = closePseudo.Contains(pseudoName);
                retVal++;
            }
            return retVal >= 3;
        }*/

        static readonly PropertyInfo PSEUDOCLASSES = typeof(StyledElement).GetProperty(nameof(PseudoClasses), BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);
        static bool TryGetPseudoClasses(StyledElement styled, out IPseudoClasses pseudoClasses)
        {
            pseudoClasses = null;
            if (styled == null)
                return false;

            var pseudosObj = PSEUDOCLASSES.GetValue(styled);
            if (pseudosObj == null)
                return false;
            
            if (pseudosObj is IPseudoClasses pseudos)
            {
                pseudoClasses = pseudos;
                return true;
            }

            return false;
        }

        bool TryGetHitButton(IntPtr hWnd, POINT lParamPos, out IntPtr ret, out Button button)
        {
            button = null;
            ret = IntPtr.Zero;
            if (TryGetClientBounds(_restoreButton, hWnd, out RECT restoreBounds) && restoreBounds.Contains(lParamPos))
            {
                //Console.WriteLine("INSIDE MAX BUTTON");
                ret = HTMAXBUTTON;
                button = _restoreButton;
                return true;
            }
            else if (TryGetClientBounds(_closeButton, hWnd, out RECT closeBounds) && closeBounds.Contains(lParamPos))
            {
                //Console.WriteLine("INSIDE CLOSE BUTTON");
                ret = HTCLOSE;
                button = _closeButton;
                return true;
            }
            else if (TryGetClientBounds(_minimizeButton, hWnd, out RECT minBounds) && minBounds.Contains(lParamPos))
            {
                //Console.WriteLine("INSIDE MINIMIZE BUTTON");
                ret = HTMINBUTTON;
                button = _minimizeButton;
                return true;
            }
            
            return false;
        }

        static bool TryGetClientBounds(Visual visual, IntPtr hWnd, /*Visual root, */out RECT resultRect)
        {
            resultRect = new RECT();
            if (visual == null)
                return false;
            /*if ((visual.TransformedBounds == null) || !visual.TransformedBounds.HasValue)
                return false;
            
            var tb = visual.TransformedBounds.Value;
            resultRect = tb.Bounds.TransformToAABB(tb.Transform);
            return true;*/




            /*var matrix = visual.TransformToVisual(root);
            if (matrix.HasValue)
            {
                resultRect = visual.Bounds.TransformToAABB(matrix.Value).Normalize();
                return true;
            }*/
            try
            {
                var oldBounds = visual.Bounds;
                var tlScreen = visual.PointToScreen(new Point(0, 0));
                var brScreen = visual.PointToScreen(new Point(oldBounds.Width, oldBounds.Height));
                var tlWin = new POINT(tlScreen.X, tlScreen.Y);
                var brWin = new POINT(brScreen.X, brScreen.Y);

                if (ScreenToClient(hWnd, ref tlWin) && ScreenToClient(hWnd, ref brWin))
                {
                    /*var tlClient = root.PointToClient(tlScreen);
                    var brClient = root.PointToClient(brScreen);
                    resultRect = new Rect(tlClient, new Size(brClient.X - tlClient.X, brClient.Y - tlClient.Y));*/
                    resultRect = new RECT(tlWin, brWin); //new Rect(tlWin.X, tlWin.Y, brWin.X - tlWin.X, brWin.Y - tlWin.Y);
                    return true;
                }
                else
                {
                    Console.WriteLine("o no");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("a wild Exception appeared! " + ex.ToString());
            }
            return false;
        }

        void ToggleMaximize()
        {
            if (!(VisualRoot is Window win))
                return;
            
            
            if (win.WindowState == WindowState.Maximized)
                win.WindowState = WindowState.Normal;
            else
                win.WindowState = WindowState.Maximized;
        }
    }
}