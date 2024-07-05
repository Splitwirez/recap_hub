using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.UITest.Views.Pages
{
    public partial class ButtonView
        : UserControl
    {
        public ButtonView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
