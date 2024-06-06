using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public class LocalPlayViewModel
        : ViewModelBase
    {
        //ObservableCollection<GameConfigViewModel> _gameConfigs = new ObservableCollection<GameConfigViewModel>();
        public ObservableCollection<GameConfigViewModel> GameConfigs
        {
            get => HubData.Instance.GameConfigs;
            /*set
            {
                var prev = HubData.Instance.GameConfigs;
                HubData.Instance.GameConfigs = value;
                if (prev != value)
                {
                    //NotifyPropertyChanged
                }
            }*/
        }


        GameConfigViewModel _selectedGameConfig = null;
        public GameConfigViewModel SelectedGameConfig
        {
            get => _selectedGameConfig;
            set => RASIC(ref _selectedGameConfig, value);
        }


        bool _showGameConfigs = true;
        public bool ShowGameConfigs
        {
            get => _showGameConfigs;
            set => RASIC(ref _showGameConfigs, value);
        }

        public LocalPlayViewModel()
             : base()
        {
            if (TimeHelper.TryGetNewest(GameConfigs, s => s.LastLaunchTime, out GameConfigViewModel lastPlayed))
                SelectedGameConfig = lastPlayed;
        }
    }
}
