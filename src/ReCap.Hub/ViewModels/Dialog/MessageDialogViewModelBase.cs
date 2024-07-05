using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public abstract class MessageDialogViewModelBase<T>
        : ViewModelBase
        , IDialogViewModel<T>
    {
        readonly TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();
        public TaskCompletionSource<T> CompletionSource
        {
            get => _tcs;
        }

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        string _content = string.Empty;
        public string Content
        {
            get => _content;
            set => RASIC(ref _content, value);
        }

        public MessageDialogViewModelBase(string title, string content, bool isCloseable)
        {
            Title = title;
            Content = content;
            IsCloseable = isCloseable;
        }

        bool _isCloseable = false;
        public bool IsCloseable
        {
            get => _isCloseable;
            set => RASIC(ref _isCloseable, value);
        }
    }
}
