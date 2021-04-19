using System;
using ReCap.Hub.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentPage
        {
            get;
            set;
        } = new HomePageViewModel();
    }
}
