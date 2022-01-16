using System;
using ReCap.Hub.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase _currentPage = new HomePageViewModel();
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set => RASIC(ref _currentPage, value);
        }
    }
}
