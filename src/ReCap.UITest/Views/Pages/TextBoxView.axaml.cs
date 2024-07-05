using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.UITest.Views.Pages
{
    public partial class TextBoxView
        : UserControl
    {
        public TextBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
