using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace ReCap.Hub.Views
{
    public partial class MainWindow : Window
    {
        const bool _INVENTORY_STYLE_TABS = false;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

#if RECAP_ONLINE
            if (_INVENTORY_STYLE_TABS)
            {
                Console.WriteLine("Inventory-style tabs");
                Classes.Add("inventoryTabs");
            }
            else
            {
                Console.WriteLine("HELIX-style tabs");
                Classes.Add("helixTabs");
            }
#else
            Width -= 194;
            Height -= 67;
#endif

            /*var layerManager = this.FindDescendantOfType<VisualLayerManager>();
            var overlayLayer = OverlayLayer.GetOverlayLayer(layerManager);
            overlayLayer.Children.Add(new DialogOverlay());*/
#if DEBUG
            //Dispatcher.UIThread.Post(() => EnsureConsistentPosition(true, false, 20), DispatcherPriority.Render);
#endif
        }


        void EnsureConsistentPosition(bool alignToRight, bool alignToBottom, int inset)
            => EnsureConsistentPosition(alignToRight, alignToBottom, inset, inset);
        void EnsureConsistentPosition(bool alignToRight, bool alignToBottom, int hInset, int vInset)
        {
            var screenBounds = Screens.ScreenFromWindow(this).Bounds;
            var winPxSize = PixelSize.FromSize(Bounds.Size, DesktopScaling);
            
            var prevPos = Position;

            int newX = EnsureConsistentPositionForAxis(winPxSize.Width, alignToRight, hInset, screenBounds.X, screenBounds.Right);
                //(screenBounds.Right - winPxSize.Width) - inset;
            int newY = EnsureConsistentPositionForAxis(winPxSize.Height, alignToBottom, vInset, screenBounds.Y, screenBounds.Bottom);
                //screenBounds.Y + inset;

            Position = new(newX, newY);
        }

        static int EnsureConsistentPositionForAxis(int windowExtent, bool alignToFar, int inset, int scNear, int scFar)
        {
            if (alignToFar)
                return (scFar - windowExtent) - inset;
            else
                return scNear + inset;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}