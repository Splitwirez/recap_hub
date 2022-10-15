using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public class GameConfigView : UserControl
    {

        public static readonly StyledProperty<bool> FlushWithLeftProperty =
            AvaloniaProperty.Register<GameConfigView, bool>(nameof(FlushWithLeft), true);
        public bool FlushWithLeft
        {
            get => GetValue(FlushWithLeftProperty);
            set => SetValue(FlushWithLeftProperty, value);
        }

        public GameConfigView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
