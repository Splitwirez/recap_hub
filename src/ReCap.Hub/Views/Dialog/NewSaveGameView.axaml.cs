using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace ReCap.Hub.Views
{
    public partial class NewSaveGameView
        : UserControl
    {
        public NewSaveGameView()
        {
            InitializeComponent();
            //TextBox textBox = this.FindControl<TextBox>("GameTitleTextBox");
            //textBox.KeyDown += GameTitleTextBox_KeyDown;
            //this.FindControl<TextBox>("GameTitleTextBox").KeyDown += GameTitleTextBox_KeyDown;
        }

        private void GameTitleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var newSaveGameVM = DataContext as ViewModels.NewSaveGameViewModel;

            if (e.Key == Key.Enter)
            {
                newSaveGameVM.Accept(null);
            }

            /*
            if (this.FindAncestorOfType<DialogOverlayView>()?.DataContext is ViewModels.DialogOverlayViewModel dlgOverlayVM)
                dlgOverlayVM.CloseCurrentDialog();
            */
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
