using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Rendering;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI
{
    public enum ResizeEdgeCell
    {
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        None
    }

    public partial class ResizeEdge : TemplatedControl, ICustomHitTest
    {
        public bool HitTest(Point point)
        {
            var bounds = Bounds;
            if (!bounds.Contains(point))
                return false;

            var ptX = point.X;
            var ptY = point.Y;

            var bt = BorderThickness;

            return HitTestEx(ptX, ptY, bt, bounds) != ResizeEdgeCell.None;
        }

        public ResizeEdgeCell HitTestEx(double x, double y)
        {
            var bounds = Bounds;
            if (!bounds.Contains(new Point(x, y)))
                return ResizeEdgeCell.None;

            Thickness borderThickness = BorderThickness;

            return HitTestEx(x, y, borderThickness, bounds, true);
        }

        ResizeEdgeCell HitTestEx(double x, double y, Thickness border, Rect bounds, bool print = false)
        {
            if (!(IsVisible || IsHitTestVisible))
                return ResizeEdgeCell.None;


            var bound = bounds.Deflate(border);
            
            double left = bound.Left;
            double top = bound.Top;
            double right = bound.Right;
            double bottom = bound.Bottom;

            /*if (print)
            {
                Debug.WriteLine($"{nameof(HitTestEx)}: ({x}, {y}), ({left}, {top}, {right}, {bottom})");
            }*/
            
            bool l = x < left;
            bool t = y < top;
            bool r = x > right;
            bool b = y > bottom;
            if (l)
            {
                if (t)
                    return ResizeEdgeCell.TopLeft;
                else if (b)
                    return ResizeEdgeCell.BottomLeft;
                else
                    return ResizeEdgeCell.Left;
            }
            else if (r)
            {
                if (t)
                    return ResizeEdgeCell.TopRight;
                else if (b)
                    return ResizeEdgeCell.BottomRight;
                else
                    return ResizeEdgeCell.Right;
            }
            else if (b)
                return ResizeEdgeCell.Bottom;
            else if (t)
                return ResizeEdgeCell.Top;
            else
                return ResizeEdgeCell.None;
        }

        //Control _
        /*protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
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


        Control SetupSide(string name, StandardCursorType cursor, WindowEdge edge, ref TemplateAppliedEventArgs e)
        {
            var control = e.NameScope.Get<Control>(name);
            control.Cursor = new Cursor(cursor);
            control.PointerPressed += (object sender, PointerPressedEventArgs ep) =>
            {
                if (VisualRoot.GetVisualRoot() is Window win)
                    win.PlatformImpl?.BeginResizeDrag(edge, ep);
            };
            return control;
        }*/
    }
}