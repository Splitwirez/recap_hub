using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public abstract class AccountViewModelBase : ViewModelBase
    {
        string _currentThreatLevel = "1-1";
        public string CurrentThreatLevel
        {
            get => _currentThreatLevel;
            set => RASIC(ref _currentThreatLevel, value);
        }


        int _currentCrogenitorLevel = 0;
        public int CurrentCrogenitorLevel
        {
            get => _currentCrogenitorLevel;
            set => RASIC(ref _currentCrogenitorLevel, value);
        }

        ObservableCollection<HeroViewModel> _heroes = new ObservableCollection<HeroViewModel>();
        public ObservableCollection<HeroViewModel> Heroes
        {
            get => _heroes;
            set => RASIC(ref _heroes, value);
        }
    }
}
