using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public class DialogOverlay : UserControl
    {
        public DialogOverlay()
        {
            InitializeComponent();

            DataContext = new DialogOverlayVM();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public class DialogOverlayVM : ViewModels.ViewModelBase
    {
        IDialogViewModel _currentDialog = null;

        public IDialogViewModel CurrentDialog
        {
            get => _currentDialog;
            set => RASIC(ref _currentDialog, value);
        }


        bool _hasDialog = false;

        public bool HasDialog
        {
            get => _hasDialog;
            set => RASIC(ref _hasDialog, value);
        }

        public DialogOverlayVM()
        {
            DialogDisplay.DialogShown += DialogDisplay_DialogShown;
        }

        private void DialogDisplay_DialogShown(object sender, DialogShownEventArgs e)
        {
            if (e != null)
            {
                CurrentDialog = e.ViewModel;
                HasDialog = CurrentDialog != null;
            }
            else
            {
                CurrentDialog = null;
                HasDialog = false;
            }
        }
    }
}