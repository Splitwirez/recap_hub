using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}