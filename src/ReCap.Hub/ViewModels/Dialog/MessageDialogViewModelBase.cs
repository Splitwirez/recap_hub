using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public abstract class MessageDialogViewModelBase<T> : ViewModelBase, IDialogViewModel<T>
    {
        TaskCompletionSource<T> _tcs = new TaskCompletionSource<T>();
        public TaskCompletionSource<T> GetCompletionSource()
            => _tcs;

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

        public MessageDialogViewModelBase(string title, string content)
        {
            title = Title;
            Content = content;
        }
    }
}
