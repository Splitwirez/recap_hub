using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        LocalPlayViewModel _localPlayVM = null;
        public LocalPlayViewModel LocalPlayVM
        {
            get => _localPlayVM;
            private set => RASIC(ref _localPlayVM, value);
        }

        public HomePageViewModel()
            : base()
        {
            LocalPlayVM = new LocalPlayViewModel()
            {
                GameConfigs = new ObservableCollection<GameConfigViewModel>()
                {
                    new GameConfigViewModel()
                    {
                        Title = "Darkspore",
                        Saves = new ObservableCollection<SaveGameViewModel>()
                        {
                            new SaveGameViewModel()
                            {
                                Title = "Campaign 1",
                                CurrentCrogenitorLevel = 10,
                                CurrentThreatLevel = "3-2"
                            },
                            new SaveGameViewModel()
                            {
                                Title = "Campaign 2",
                                CurrentCrogenitorLevel = 10,
                                CurrentThreatLevel = "3-1"
                            }
                        }
                    },
                    new GameConfigViewModel()
                    {
                        Title = "Campaign mods",
                    }
                }
            };

            if (LocalPlayVM.GameConfigs.Count > 0)
                LocalPlayVM.SelectedGameConfig = LocalPlayVM.GameConfigs[0];
            foreach (GameConfigViewModel gameConfig in LocalPlayVM.GameConfigs)
            {
                if (gameConfig.Saves.Count > 0)
                    gameConfig.SelectedSave = gameConfig.Saves[0];
            }
        }
    }
}
