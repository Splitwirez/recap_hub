using System;
using System.Diagnostics;
using ReCap.Hub.ViewModels;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ReCap.Hub.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase _currentPage = null;

        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => RASIC(ref _currentPage, value);
        }

        bool _enableMP = false;
        public bool EnableMP
        {
            get => _enableMP;
            private set => RASIC(ref _enableMP, value);
        }


        public MainWindowViewModel(ViewModelBase currentPage)
            : this()
        {
            CurrentPage = currentPage;
        }

        public MainWindowViewModel()
            : base()
        {
#if RECAP_ONLINE
            Debug.WriteLine("RECAP_ONLINE");
            EnableMP = true;
#endif
        }
    }
}
