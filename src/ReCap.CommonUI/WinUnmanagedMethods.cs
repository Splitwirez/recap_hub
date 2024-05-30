using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;


namespace ReCap.CommonUI
{
    public static class WinUnmanagedMethods
    {
        public const uint WM_NCHITTEST = 0x0084;
        public const uint WM_NCLBUTTONDOWN = 0x00A1;
        public const uint WM_NCLBUTTONUP = 0x00A2;
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_NCMOUSEMOVE = 0x00A0;
        public const uint WM_MOUSEMOVE = 0x00A0;
        public const uint WM_NCCALCSIZE = 0x0083;
        public const uint WM_STYLECHANGING = 0x007C;


        public static readonly IntPtr HTBORDER = new IntPtr(18);
        public static readonly IntPtr HTBOTTOM = new IntPtr(15);
        public static readonly IntPtr HTBOTTOMLEFT = new IntPtr(16);
        public static readonly IntPtr HTBOTTOMRIGHT = new IntPtr(17);
        public static readonly IntPtr HTCAPTION = new IntPtr(2);
        public static readonly IntPtr HTCLIENT = new IntPtr(1);
        public static readonly IntPtr HTCLOSE = new IntPtr(20);
        public static readonly IntPtr HTERROR = new IntPtr(-2);
        public static readonly IntPtr HTGROWBOX = new IntPtr(4);
        public static readonly IntPtr HTHELP = new IntPtr(21);
        public static readonly IntPtr HTHSCROLL = new IntPtr(6);
        public static readonly IntPtr HTLEFT = new IntPtr(10);
        public static readonly IntPtr HTMENU = new IntPtr(5);
        public static readonly IntPtr HTMAXBUTTON = new IntPtr(9);
        public static readonly IntPtr HTMINBUTTON = new IntPtr(8);
        public static readonly IntPtr HTNOWHERE = new IntPtr(0);
        public static readonly IntPtr HTREDUCE = new IntPtr(8);
        public static readonly IntPtr HTRIGHT = new IntPtr(11);
        public static readonly IntPtr HTSIZE = new IntPtr(4);
        public static readonly IntPtr HTSYSMENU = new IntPtr(3);
        public static readonly IntPtr HTTOP = new IntPtr(12);
        public static readonly IntPtr HTTOPLEFT = new IntPtr(13);
        public static readonly IntPtr HTTOPRIGHT = new IntPtr(14);
        public static readonly IntPtr HTTRANSPARENT = new IntPtr(-1);
        public static readonly IntPtr HTVSCROLL = new IntPtr(7);
        public static readonly IntPtr HTZOOM = new IntPtr(9);



        public static readonly IntPtr SC_CLOSE = new IntPtr(0xF060);
        public static readonly IntPtr SC_MINIMIZE = new IntPtr(0xF020);
        public static readonly IntPtr SC_MAXIMIZE = new IntPtr(0xF030);


        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);


        public enum WindowLongParam : int
        {
            GWL_WNDPROC = -4,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_EXSTYLE = -20,
            GWL_USERDATA = -21
        }

        public enum ClassLongIndex : int
        {
            GCLP_MENUNAME = -8,
            GCLP_HBRBACKGROUND = -10,
            GCLP_HCURSOR = -12,
            GCLP_HICON = -14,
            GCLP_HMODULE = -16,
            GCL_CBWNDEXTRA = -18,
            GCL_CBCLSEXTRA = -20,
            GCLP_WNDPROC = -24,
            GCL_STYLE = -26,
            GCLP_HICONSM = -34,
            GCW_ATOM = -32
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASSEX
        {
            public int cbSize;
            public int style;
            public WndProcDelegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong32b(IntPtr hWnd, int nIndex);

        public static uint GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32b(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLong64b(IntPtr hWnd, int nIndex, IntPtr value);

        public static uint SetWindowLong(IntPtr hWnd, int nIndex, uint value)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLong32b(hWnd, nIndex, value);
            }
            else
            {
                return (uint)SetWindowLong64b(hWnd, nIndex, new IntPtr((uint)value)).ToInt32();
            }
        }

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr handle)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(SetWindowLong32b(hWnd, nIndex, (uint)handle.ToInt32()));
            }
            else
            {
                return SetWindowLong64b(hWnd, nIndex, handle);
            }
        }

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public POINT(IntPtr param)
            {
                var iParam = param.ToInt32();
                X = iParam & 0x0000ffff;
                Y = iParam >> 16;
            }

            public IntPtr ToWMParam()
            {
                //https://social.msdn.microsoft.com/Forums/vstudio/en-US/d9965d14-34ac-48ee-ae4f-85cee689cc33/how-to-make-lparam?forum=vbgeneral
                
                var retList = new List<byte>();
                byte[] xByte = BitConverter.GetBytes(X);
                byte[] yByte = BitConverter.GetBytes(Y);
                retList.Add(xByte[0]);
                retList.Add(xByte[1]);
                retList.Add(yByte[0]);
                retList.Add(yByte[1]);
                return new IntPtr(BitConverter.ToInt32(retList.ToArray(), 0));
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(/*IntPtr lpPrevWndFunc, */IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        //(IntPtr hWnd, uint Msg, nuint wParam, StringBuilder lParam);

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width => right - left;
            public int Height => bottom - top;
            /*public RECT(Rect rect)
            {
                left = (int)rect.X;
                top = (int)rect.Y;
                right = (int)(rect.X + rect.Width);
                bottom = (int)(rect.Y + rect.Height);
            }*/

            public RECT(POINT topLeft, POINT bottomRight)
            {
                left = topLeft.X;
                top = topLeft.Y;
                right = bottomRight.X;
                bottom = bottomRight.Y;
            }

            public void Offset(POINT pt)
            {
                left += pt.X;
                right += pt.X;
                top += pt.Y;
                bottom += pt.Y;
            }

            public bool Contains(POINT point)
            {
                return
                    (point.X >= left)
                    && (point.Y >= top)
                    && (point.X < right)
                    && (point.Y < bottom)
                ;
            }
        }
        
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);


        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public RECT[] rgrc;
            public WINDOWPOS lppos;
        }


        public static readonly IntPtr WVR_REDRAW = new IntPtr(0x0300);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public const uint WS_CAPTION = 0x00C00000;


        [StructLayout(LayoutKind.Sequential)]
        internal struct RTL_OSVERSIONINFOEX
        {
            internal uint dwOSVersionInfoSize;
            internal uint dwMajorVersion;
            internal uint dwMinorVersion;
            internal uint dwBuildNumber;
            internal uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szCSDVersion;
        }

        [DllImport("ntdll.dll")]
        internal static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);


        [StructLayout(LayoutKind.Sequential)]
        internal struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }
        [DllImport("dwmapi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
    }
}