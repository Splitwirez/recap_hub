using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace ReCap.Hub.Views
{
    public partial class GameConfigView : UserControl
    {
        ContentControl _saveGamesPane = null;


        public static readonly StyledProperty<bool> FlushWithLeftProperty =
            AvaloniaProperty.Register<GameConfigView, bool>(nameof(FlushWithLeft), true);
        public bool FlushWithLeft
        {
            get => GetValue(FlushWithLeftProperty);
            set => SetValue(FlushWithLeftProperty, value);
        }

        static GameConfigView()
        {
            FlushWithLeftProperty.Changed.AddClassHandler<GameConfigView>((s, e) =>
            {
                if (e.NewValue is bool newValue)
                    s.OnFlushWithLeftPropertyChanged(newValue);
            });
        }
        
        public GameConfigView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _saveGamesPane = this.Find<ContentControl>("SaveGamesPane");
        }

        /*private void SaveGamesPaneListItem_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            ListBoxItem item = null;
            if (sender is ListBoxItem sItem)
                item = sItem;
            else if ((sender is IStyleable styl) && (styl.TemplatedParent is ListBoxItem tItem))
                item = tItem;
            if (item == null)
                return;

            item.PointerEnter += SaveGamesPaneListItem_PointerEnter;
        }
        private void SaveGamesPaneListItem_PointerEnter(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if (!(sender is ListBoxItem item))
                return;

            var flyout = FlyoutBase.GetAttachedFlyout(item);
            flyout.ShowAt(item);
            item.Focus();
            item.PointerLeave += SaveGamesPaneListItem_PointerLeave;
        }

        private void SaveGamesPaneListItem_PointerLeave(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if (!(sender is ListBoxItem item))
                return;

            var flyout = FlyoutBase.GetAttachedFlyout(item);
            flyout.Hide();
        }*/

        const string PANE_TOGGLE_CLASS = "outermost";
        void OnFlushWithLeftPropertyChanged(bool newValue)
        {
            if (newValue && (!_saveGamesPane.Classes.Contains(PANE_TOGGLE_CLASS))) //if (e.NewValue is bool flushWithLeft)
                _saveGamesPane.Classes.Add(PANE_TOGGLE_CLASS);
            else if (_saveGamesPane.Classes.Contains(PANE_TOGGLE_CLASS))
                _saveGamesPane.Classes.Remove(PANE_TOGGLE_CLASS);
        }
    }
}
