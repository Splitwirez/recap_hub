using System;
using System.Diagnostics;
using ReCap.Hub.ViewModels;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public class MainViewModel
        : ViewModelBase
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


        public async Task ShowPreferencesCommand(object _)
        {
            await DialogDisplay.ShowDialog(new PreferencesPageViewModel());
        }


        public MainViewModel(ViewModelBase currentPage = null)
            : base()
        {
            if (currentPage != null)
                CurrentPage = currentPage;
            else
                CurrentPage = _defaultPage;

#if RECAP_ONLINE
            Debug.WriteLine("RECAP_ONLINE");
            EnableMP = true;
#endif
        }

#if !RECAP_ONLINE
        public override Type GetViewType()
            => _defaultPage.GetViewType();
#endif
        /*
#if RECAP_ONLINE
            new MainView()
#else
            new HomePa
#endif
        */

        static ViewModelBase CreateDefaultPage()
            =>
#if RECAP_ONLINE
                new HomePageViewModel()
#else
                Data.HubData.Instance.GameConfigs[0]
#endif
            ;
        readonly ViewModelBase _defaultPage = CreateDefaultPage();
    }
}
