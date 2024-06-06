using ReCap.Hub.ViewModels;

namespace ReCap.UITest.ViewModels
{
    public class PageTabViewModel
        : ViewModelBase
    {
        ViewModelBase _contentVM = null;
        public ViewModelBase ContentVM
        {
            get => _contentVM;
            private set => RASIC(ref _contentVM, value);
        }

        string _title = null;
        public string Title
        {
            get => _title;
            private set => RASIC(ref _title, value);
        }
        

        public PageTabViewModel(string title, ViewModelBase contentVM)
            : base()
        {
            Title = title;
            ContentVM = contentVM;
        }
    }
}