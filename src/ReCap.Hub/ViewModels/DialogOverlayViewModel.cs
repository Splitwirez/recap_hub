using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public class DialogOverlayViewModel
        : ViewModelBase
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

        public DialogOverlayViewModel()
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

        public void CloseCommand(object _)
            => CloseCurrentDialog();

        public void CloseCurrentDialog()
        {
            if (CurrentDialog == null)
                return;

            if (!CurrentDialog.IsCloseable)
                return;

            CurrentDialog.OnClosed();
            //CurrentDialog.GetCompletionSource
        }
    }
}
