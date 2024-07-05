using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    class TextBoxDialogViewModel : MessageDialogViewModelBase<string>
    {
        string _value = string.Empty;
        public string Value
        {
            get => _value;
            set => RASIC(ref _value, value);
        }


        public void AcceptCommand(object parameter)
            => CompletionSource.TrySetResult(Value);

        public void CancelCommand(object parameter)
            => CompletionSource.TrySetResult(null);

        public TextBoxDialogViewModel(string title, string content, bool isCloseable)
            : base(title, content, isCloseable)
        { }
        public TextBoxDialogViewModel(string title, string content, string initialValue, bool isCloseable)
            : this(title, content, isCloseable)
        {
            Value = initialValue;
        }
    }
}
