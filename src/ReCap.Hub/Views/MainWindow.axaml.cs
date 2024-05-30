using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace ReCap.Hub.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

#if !RECAP_ONLINE
            Width -= 194;
            Height -= 67;
#endif

            /*var layerManager = this.FindDescendantOfType<VisualLayerManager>();
            var overlayLayer = OverlayLayer.GetOverlayLayer(layerManager);
            overlayLayer.Children.Add(new DialogOverlay());*/
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}