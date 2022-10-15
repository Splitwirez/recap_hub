using System;
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

        public MainWindowViewModel(ViewModelBase currentPage)
            : base()
        {
            CurrentPage = currentPage;
        }

        public MainWindowViewModel()
            : base()
        {
            HomePageViewModel currentPage = new HomePageViewModel();
            CurrentPage = currentPage
#if LOCAL_ONLY
            .LocalPlayVM.GameConfigs.First()
#endif
            ;
        }
    }
}
