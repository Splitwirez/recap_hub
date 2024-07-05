using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.UITest.Views.Pages
{
    public partial class ComboBoxView
        : UserControl
    {
        public ComboBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
