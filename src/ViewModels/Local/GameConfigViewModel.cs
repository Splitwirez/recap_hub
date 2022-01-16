using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class GameConfigViewModel : ViewModelBase
    {
        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }


        ObservableCollection<SaveGameViewModel> _saves = new ObservableCollection<SaveGameViewModel>();
        public ObservableCollection<SaveGameViewModel> Saves
        {
            get => _saves;
            set => RASIC(ref _saves, value);
        }


        SaveGameViewModel _selectedSave = null;
        public SaveGameViewModel SelectedSave
        {
            get => _selectedSave;
            set => RASIC(ref _selectedSave, value);
        }


        public GameConfigViewModel()
            : base()
        {
            if (Saves.Count > 0)
                SelectedSave = Saves[0];
        }
    }
}
