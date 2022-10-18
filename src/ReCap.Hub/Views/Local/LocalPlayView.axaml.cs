using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public class LocalPlayView : UserControl
    {
        ContentControl _gameConfigsPane = null;
        public LocalPlayView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _gameConfigsPane = this.Find<ContentControl>("GameConfigsPane");
        }
    }
}
