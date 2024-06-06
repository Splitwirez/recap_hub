using System;
using System.Collections.ObjectModel;

namespace ReCap.Hub.ViewModels
{
    public class OnlinePlayViewModel
        : ViewModelBase
    {
        ObservableCollection<ForeignServerViewModel> _servers = new ObservableCollection<ForeignServerViewModel>();
        public ObservableCollection<ForeignServerViewModel> Servers
        {
            get => _servers;
        }


        ForeignServerViewModel _selectedServer = null;
        public ForeignServerViewModel SelectedServer
        {
            get => _selectedServer;
            set => RASIC(ref _selectedServer, value);
        }

        public OnlinePlayViewModel()
             : base()
        {
            if (TimeHelper.TryGetNewest(Servers, s => s.LastLaunchTime, out ForeignServerViewModel lastPlayed))
                SelectedServer = lastPlayed;
        }
    }
}
