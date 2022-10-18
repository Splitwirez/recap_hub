using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    class YesNoDialogViewModel : MessageDialogViewModelBase<bool>
    {
        public void YesCommand(object parameter)
            => GetCompletionSource().TrySetResult(true);

        public void NoCommand(object parameter)
            => GetCompletionSource().TrySetResult(false);

        public YesNoDialogViewModel(string title, string content)
            : base(title, content)
        { }
    }
}
