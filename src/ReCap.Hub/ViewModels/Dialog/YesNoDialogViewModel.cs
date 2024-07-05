using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public class YesNoDialogViewModel
        : MessageDialogViewModelBase<bool>
    {
        public void YesCommand(object parameter)
            => CompletionSource.TrySetResult(true);

        public void NoCommand(object parameter)
            => CompletionSource.TrySetResult(false);

        public YesNoDialogViewModel(string title, string content, bool isCloseable = false)
            : base(title, content, isCloseable)
        { }
    }
}
