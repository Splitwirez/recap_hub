using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI
{
    public partial class ResizeEdge : TemplatedControl
    {
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            SetupSide("Left_top", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Left_mid", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Left_bottom", StandardCursorType.LeftSide, WindowEdge.West, ref e);
            SetupSide("Right_top", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Right_mid", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Right_bottom", StandardCursorType.RightSide, WindowEdge.East, ref e);
            SetupSide("Top", StandardCursorType.TopSide, WindowEdge.North, ref e);
            SetupSide("Bottom", StandardCursorType.BottomSide, WindowEdge.South, ref e);
            SetupSide("TopLeft", StandardCursorType.TopLeftCorner, WindowEdge.NorthWest, ref e);
            SetupSide("TopRight", StandardCursorType.TopRightCorner, WindowEdge.NorthEast, ref e);
            SetupSide("BottomLeft", StandardCursorType.BottomLeftCorner, WindowEdge.SouthWest, ref e);
            SetupSide("BottomRight", StandardCursorType.BottomRightCorner, WindowEdge.SouthEast, ref e);
        }


        void SetupSide(string name, StandardCursorType cursor, WindowEdge edge, ref TemplateAppliedEventArgs e)
        {
            var control = e.NameScope.Get<Control>(name);
            control.Cursor = new Cursor(cursor);
            control.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.PlatformImpl?.BeginResizeDrag(edge, ep);
            };
        }
    }
}