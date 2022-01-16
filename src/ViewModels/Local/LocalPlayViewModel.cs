using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class LocalPlayViewModel : ViewModelBase
    {
        ObservableCollection<GameConfigViewModel> _gameConfigs = new ObservableCollection<GameConfigViewModel>();
        public ObservableCollection<GameConfigViewModel> GameConfigs
        {
            get => _gameConfigs;
            set => RASIC(ref _gameConfigs, value);
        }


        GameConfigViewModel _selectedGameConfig = null;
        public GameConfigViewModel SelectedGameConfig
        {
            get => _selectedGameConfig;
            set => RASIC(ref _selectedGameConfig, value);
        }

        public LocalPlayViewModel()
             : base()
        {

        }
    }
}
