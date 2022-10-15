using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public abstract partial class AccountViewModelBase : ViewModelBase
    {
        string _currentThreatLevel = "1-1";
        public string CurrentThreatLevel
        {
            get => _currentThreatLevel;
            set => RASIC(ref _currentThreatLevel, value);
        }


        public const string CROGENITOR_LEVEL_EL = "level";
        int _crogenitorLevel = 0;
        public int CrogenitorLevel
        {
            get => _crogenitorLevel;
            set => RASIC(ref _crogenitorLevel, value);
        }

        ObservableCollection<HeroViewModel> _heroes = new ObservableCollection<HeroViewModel>();
        public ObservableCollection<HeroViewModel> Heroes
        {
            get => _heroes;
            set => RASIC(ref _heroes, value);
        }
    }
}
