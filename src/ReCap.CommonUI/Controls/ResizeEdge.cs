using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Rendering;
using Avalonia.Visuals;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI
{
    public enum ResizeEdgeCell
    {
        Left = WindowEdge.West,
        TopLeft = WindowEdge.NorthWest,
        Top = WindowEdge.North,
        TopRight = WindowEdge.NorthEast,
        Right = WindowEdge.East,
        BottomRight = WindowEdge.SouthEast,
        Bottom = WindowEdge.South,
        BottomLeft = WindowEdge.SouthWest,
        None = -1
    }

    public partial class ResizeEdge : TemplatedControl, ICustomHitTest
    {
        readonly bool _onWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        Window _rootWindow = null;
        static readonly ReadOnlyDictionary<ResizeEdgeCell, string> _edgeCellToElementNamesMap = null;
        static readonly ReadOnlyDictionary<ResizeEdgeCell, Cursor> _edgeCellToCursorsMap = null;
        static readonly Dictionary<ResizeEdgeCell, InputElement> _edgeCellElements = new Dictionary<ResizeEdgeCell, InputElement>();
        static readonly Dictionary<InputElement, ResizeEdgeCell> _edgeElementCells = new Dictionary<InputElement, ResizeEdgeCell>();


        //bool _doHoverHitTest = false;
        Thickness _borderThickness = new Thickness();
        Rect _rawBounds = new Rect();
        Rect _bounds = new Rect();
        readonly Action<AvaloniaPropertyChangedEventArgs> _whenWindowStateChanged = null;
        IDisposable _onNextWindowStateChanged = null;
        static ResizeEdge()
        {
            BorderThicknessProperty.Changed.AddClassHandler<ResizeEdge>((s, e) => s.RefreshCache(e.GetNewValue<Thickness>()));
            BoundsProperty.Changed.AddClassHandler<ResizeEdge>((s, e) => s.RefreshCache(e.GetNewValue<Rect>()));
            

            var edges = Enum.GetValues(typeof(ResizeEdgeCell)).Cast<ResizeEdgeCell>().ToList();
            //var edgeNames = Enum.GetNames(typeof(ResizeEdgeCell)).ToList();
            if (!edges.Remove(ResizeEdgeCell.None))
                throw new Exception("YOU FOOL");
            
            var edgeCellToElementNamesMap = new Dictionary<ResizeEdgeCell, string>();
            var edgeCellToCursorsMap = new Dictionary<ResizeEdgeCell, Cursor>();
            foreach (ResizeEdgeCell edge in edges)
            {
                string edgeName = Enum.GetName(typeof(ResizeEdgeCell), edge);
                edgeCellToElementNamesMap.Add(edge, $"PART_{edgeName}");
                string cursorName = edgeName;
                if (edgeName.Skip(1).Count(c => char.IsUpper(c)) > 0)
                    cursorName += "Corner";
                else
                    cursorName += "Side";
                
                if (Enum.TryParse<StandardCursorType>(cursorName, out StandardCursorType stdCursor))
                    edgeCellToCursorsMap.Add(edge, new Cursor(stdCursor));
            }
            _edgeCellToElementNamesMap = new ReadOnlyDictionary<ResizeEdgeCell, string>(edgeCellToElementNamesMap);
            _edgeCellToCursorsMap = new ReadOnlyDictionary<ResizeEdgeCell, Cursor>(edgeCellToCursorsMap);
        }

        public ResizeEdge()
            : base()
        {
            _whenWindowStateChanged = e => OnRootWindowStateChanged(e.GetNewValue<WindowState>());

            RefreshCache(BorderThickness, Bounds);
        }

        protected virtual void OnRootWindowStateChanged(WindowState state)
        {
            IsVisible = ((state != WindowState.Maximized) && (state != WindowState.FullScreen));
        }

        void RefreshCachedWindow(Window rootWindow)
        {
            if (_onNextWindowStateChanged != null)
            {
                _onNextWindowStateChanged.Dispose();
                _onNextWindowStateChanged = null;
            }

            _rootWindow = rootWindow;
            
            if (_rootWindow != null)
            {
                _onNextWindowStateChanged = _rootWindow
                    .GetPropertyChangedObservable(Window.WindowStateProperty)
                    .Subscribe(_whenWindowStateChanged)
                ;
                
                OnRootWindowStateChanged(_rootWindow.WindowState);
            }

            RefreshCache();
        }
        void RefreshCache(Thickness rawBorder, Rect rawBounds)
        {
            _borderThickness = rawBorder;
            _rawBounds = rawBounds;
            RefreshCache();
        }
        void RefreshCache(Rect rawBounds)
        {
            _rawBounds = rawBounds;
            RefreshCache();
        }
        void RefreshCache(Thickness rawBorder)
        {
            _borderThickness = rawBorder;
            RefreshCache();
        }
        void RefreshCache()
        {
            _bounds = _rawBounds.Deflate(_borderThickness);
            /*if (_rootWindow == null)
            {
                Debug.WriteLine("_rootWindow == null");
                return;
            }
            
            var matrix = this.TransformToVisual(_rootWindow);
            if (!matrix.HasValue)
            {
                Debug.WriteLine("!matrix.HasValue");
                return;
            }
            _bounds = _bounds.TransformToAABB(matrix.Value);*/
        }

        public bool HitTest(Point point)
        {
            var bounds = Bounds;
            if (!bounds.Contains(point))
                return false;

            var ptX = point.X;
            var ptY = point.Y;

            /*var bt = BorderThickness;

            return HitTestEx(ptX, ptY, bt, bounds) != ResizeEdgeCell.None;*/
            return HitTestEx(ptX, ptY) != ResizeEdgeCell.None;
        }

        public ResizeEdgeCell HitTestEx(Point point)
            => HitTestEx(point.X, point.Y);
        
        public ResizeEdgeCell HitTestEx(double x, double y)
        {
            if (!(IsVisible || IsHitTestVisible))
                return ResizeEdgeCell.None;

            if (!_bounds.Contains(new Point(x, y)))
                return ResizeEdgeCell.None;


            double left = _bounds.Left;
            double top = _bounds.Top;
            double right = _bounds.Right;
            double bottom = _bounds.Bottom;
            
            
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
            
            
            return ResizeEdgeCell.None;
        }



        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            if (e.Root is Window window)
                RefreshCachedWindow(window);
        }
        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            if (_rootWindow != null)
                RefreshCachedWindow(null);
        }


        /*protected override void OnPointerEntered(PointerEventArgs e)
        {
            base.OnPointerEntered(e);
            Debug.WriteLine("Pointer entered");
            _doHoverHitTest = true;
        }
        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            Debug.WriteLine("Pointer left");
            _doHoverHitTest = false;
        }*/

        /*protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (!_doHoverHitTest)
                return;
            
            var pos = e.GetPosition(this);
            var cell = HitTestEx_PointInRootCoords(pos);
            SetEdgeHoverClass(cell);
        }*/
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (_onWindows)
                return;
            if (_rootWindow == null)
                return;
            
            var pos = e.GetPosition(this);
            var cell = HitTestEx(pos);
            if (cell != ResizeEdgeCell.None)
                BeginResizeDrag(cell, e);
        }

        void BeginResizeDrag(ResizeEdgeCell cell, PointerPressedEventArgs e)
        {
            var edge = (WindowEdge)cell;
            BeginResizeDrag(edge, e);
        }
        void BeginResizeDrag(WindowEdge edge, PointerPressedEventArgs e)
        {
            //_rootWindow.BeginResizeDrag(edge, e);
            _rootWindow.PlatformImpl?.BeginResizeDrag(edge, e);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            foreach (var edge in _edgeCellElements)
            {
                var val = edge.Value;
                if (val == null)
                    continue;
                
                if (!_onWindows)
                {
                    var ctrl = edge.Value;
                    ctrl.PointerPressed -= EdgeElement_PointerPressed;
                }
            }
            _edgeCellElements.Clear();
            _edgeElementCells.Clear();


            foreach (var edge in _edgeCellToElementNamesMap)
            {
                var key = edge.Key;
                var ctrl = e.NameScope.Get<InputElement>(edge.Value);
                
                if (_onWindows)
                    ctrl.IsHitTestVisible = false;
                else
                {
                    ctrl.PointerPressed += EdgeElement_PointerPressed;
                    ctrl.Cursor = _edgeCellToCursorsMap[key];
                }
                
                _edgeCellElements.Add(key, ctrl);
                _edgeElementCells.Add(ctrl, key);
            }
        }


        protected virtual void EdgeElement_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is InputElement element)
                BeginResizeDrag(_edgeElementCells[element], e);
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