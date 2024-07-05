using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ReCap.Hub.Localization.Keys;

namespace ReCap.Hub.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
    {
        /*
        LocalPlayViewModel _localPlayVM = null;
        public LocalPlayViewModel LocalPlayVM
        {
            get => _localPlayVM;
            private set => RASIC(ref _localPlayVM, value);
        }

        OnlinePlayViewModel _onlinePlayVM = null;
        public OnlinePlayViewModel OnlinePlayVM
        {
            get => _onlinePlayVM;
            private set => RASIC(ref _onlinePlayVM, value);
        }
        */
        ObservableCollection<TabViewModel> _tabs = new ObservableCollection<TabViewModel>()
        {
            new TabViewModel(HomePage.OfflinePlayTabHeader, "LocalPlayTabIcon", new LocalPlayViewModel()),
            new TabViewModel(HomePage.OnlinePlayTabHeader, "OnlinePlayTabIcon", new OnlinePlayViewModel()),
            //new TabViewModel("SAMPLE TEXT", null),
        };
        public ObservableCollection<TabViewModel> Tabs
        {
            get => _tabs;
        }

        public HomePageViewModel()
            : base()
        {
            /*
            LocalPlayVM = new LocalPlayViewModel();
            OnlinePlayVM = new OnlinePlayViewModel();
            */
        }
    }
}
