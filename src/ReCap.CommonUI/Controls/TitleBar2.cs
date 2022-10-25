using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input.Raw;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Threading;
using System.IO;
using System.Threading.Tasks;

using static ReCap.CommonUI.WinUnmanagedMethods;

namespace ReCap.CommonUI
{
    [TemplatePart("PART_CaptionButtons", typeof(CaptionButtons))]
    public partial class TitleBar2 : ContentControl
    {
        static bool DefaultToLeftSideButtons => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        public static readonly StyledProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.Register<TitleBar2, bool>(nameof(LeftSideButtons), defaultValue: DefaultToLeftSideButtons);
        
        public bool LeftSideButtons
        {
            get => GetValue(LeftSideButtonsProperty);
            set => SetValue(LeftSideButtonsProperty, value);
        }

        CaptionButtons _captionButtons = null;
        Button _minimiseButton = null;
        Button _restoreButton = null;
        Button _closeButton = null;
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

            var dragGrip = e.NameScope.Find<TemplatedControl>("PART_DragGrip");
            
            dragGrip.PointerPressed += (s, a) =>
            {
                if (a.ClickCount <= 1)
                {
                    if (VisualRoot.GetVisualRoot() is Window win)
                        win.BeginMoveDrag(a);
                }
            };

            dragGrip.DoubleTapped += (s, a) => ToggleMaximize();

            _captionButtons?.Detach();

            _captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");
            _captionButtons.TemplateApplied += CaptionButtons_TemplateApplied;
            /*var maxButton = _captionButtons.FindNameScope().Find<Button>("PART_RestoreButton");
            
            maxButton.IsVisible = true;*/


            if (VisualRoot.GetVisualRoot() is Window window)
            {
                _captionButtons?.Attach(window);
            }
        }

        

        private WndProcDelegate _wndProc;
        IntPtr _newProcPtr = IntPtr.Zero;
        IntPtr _oldWndProc = IntPtr.Zero;
        void CaptionButtons_TemplateApplied(object sender, TemplateAppliedEventArgs e)
        {
            _minimiseButton = e.NameScope.Find<Button>("PART_MinimiseButton");
            _restoreButton = e.NameScope.Find<Button>("PART_RestoreButton");
            _closeButton = e.NameScope.Find<Button>("PART_CloseButton");
            


            //https://gist.github.com/AlexanderBaggett/d1504da93727a1778e8b5b3453946fc1
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) //OperatingSystem.IsWindows())
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                {
                    IntPtr hWnd = win.PlatformImpl.Handle.Handle;
                    //SetWindowLongPtr(hWnd, (int)WindowLongParam.GWL_WNDPROC, );
                    
                    _wndProc = CaptionButtonsWndProc; //new WndProc(CaptionButtonsWndProc);
                    _newProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);
                    _oldWndProc = SetWindowLongPtr(hWnd, (int)(WindowLongParam.GWL_WNDPROC), _newProcPtr);
                    // = Marshal.GetDelegateForFunctionPointer<WndProcDelegate>(_oldWndProc);
                }
            }
            (sender as CaptionButtons).TemplateApplied -= CaptionButtons_TemplateApplied;
        }

        bool _ncLButtonDown = false;
        public IntPtr CaptionButtonsWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            var root = VisualRoot.GetVisualRoot();
            if ((root != null) && (root is Window win) && TryHandleWmNcMessages(win, hWnd, msg, ref wParam, ref lParam, out IntPtr wmNcRet))
            {
                return wmNcRet;
            }
            
            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
            //return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        bool TryHandleWmNcMessages(Window win, IntPtr hWnd, uint msg, ref IntPtr wParam, ref IntPtr lParam, out IntPtr ret)
        {
            ret = IntPtr.Zero;
            if (msg == WM_NCHITTEST)
            {
                //var lParamPosRaw = new PixelPoint(x, y);
                var lParamPos = new POINT(lParam);

                if (ScreenToClient(hWnd, ref lParamPos))
                {
                    /*var lParamPos = Avalonia.VisualExtensions.PointToClient(win, lParamPosRaw)
                    ;*/
                    Console.WriteLine($"WM_NCHITTEST: {lParamPos.X}, {lParamPos.Y}");
                    //if (TryGetTransformedBounds(_restoreButton, out Rect restoreBounds) && restoreBounds.ContainsExclusive(lParamPos))
                    if (TryGetHitButton(hWnd, lParamPos, out ret))
                    {
                        //DefWindowProc(hWnd, msg, wParam, lParam)
                        return true;
                    }
                    //VisualExtensions.PointToClient()
                    //VisualExtensions.
                }
                else
                    Console.WriteLine("ScreenToClient :b:roke");
            }
            else if (msg == WM_SYSCOMMAND)
            {
                string command = null;
                if (wParam == SC_CLOSE)
                    command = nameof(SC_CLOSE);
                else if (wParam == SC_MAXIMIZE)
                    command = nameof(SC_MAXIMIZE);
                else if (wParam == SC_MINIMIZE)
                    command = nameof(SC_MINIMIZE);
                if (command != null)
                {
                    Console.WriteLine($"WM_SYSCOMMAND: {command}");
                    //DefWindowProc(hWnd, msg, wParam, lParam);
                    /*CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
                    return true;*/
                }
                else
                {
                    Console.WriteLine($"WM_SYSCOMMAND");
                    //return false;
                }
            }
            else
            {
                bool ncLeftBtnDown = msg == WM_NCLBUTTONDOWN;
                bool ncLeftBtnUp = msg == WM_NCLBUTTONUP;
                if (ncLeftBtnDown || ncLeftBtnUp)
                {
                    IntPtr newWParam = wParam; //SendMessage(hWnd, WM_NCHITTEST, wParam, lParam);
                    bool max = (newWParam == HTMAXBUTTON);
                    bool min = (!max) && (newWParam == HTMINBUTTON);
                    bool close = (!max) && (!min) && (newWParam == HTCLOSE);
                    if (max || min || close)
                    {
                        //_ncLButtonDown = ncLeftBtnDown;
                        //DefWindowProc(hWnd, msg, wParam, lParam);
                        var clientMsg = ncLeftBtnDown
                            ? WM_LBUTTONDOWN
                            : WM_LBUTTONUP
                        ;
                            //CallWindowProc(_oldWndProc, hWnd, msg, newWParam, lParam);
                            //IntPtr newWParam = SendMessage(hWnd, WM_NCHITTEST, wParam, lParam);
                        POINT point = new POINT(lParam);
                        if (ScreenToClient(hWnd, ref point))
                        {
                            SendMessage(hWnd, clientMsg, IntPtr.Zero, point.ToWMParam());
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        bool TryGetHitButton(IntPtr hWnd, POINT lParamPos, out IntPtr ret)
        {
            ret = IntPtr.Zero;
            if (TryGetClientBounds(_restoreButton, hWnd, out RECT restoreBounds) && restoreBounds.Contains(lParamPos))
            {
                Console.WriteLine("INSIDE MAX BUTTON");
                ret = HTMAXBUTTON;
                return true;
            }
            else if (TryGetClientBounds(_closeButton, hWnd, out RECT closeBounds) && closeBounds.Contains(lParamPos))
            {
                Console.WriteLine("INSIDE CLOSE BUTTON");
                ret = HTCLOSE;
                return true;
            }
            else if (TryGetClientBounds(_minimiseButton, hWnd, out RECT minBounds) && minBounds.Contains(lParamPos))
            {
                Console.WriteLine("INSIDE MINIMIZE BUTTON");
                ret = HTMINBUTTON;
                return true;
            }
            
            return false;
        }

        static bool TryGetClientBounds(Visual visual, IntPtr hWnd, /*Visual root, */out RECT resultRect)
        {
            resultRect = new RECT();
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
            if (VisualRoot.GetVisualRoot() is Window win)
            {
                if (win.WindowState == WindowState.Maximized)
                    win.WindowState = WindowState.Normal;
                else
                    win.WindowState = WindowState.Maximized;
            }
        }
    }
}