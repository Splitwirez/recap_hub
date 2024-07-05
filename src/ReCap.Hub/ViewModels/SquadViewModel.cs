using Avalonia.Media.Imaging;
using ReCap.Hub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.ViewModels
{
    public class SquadViewModel : ViewModelBase
    {
        readonly DeckModel _model = null;
        public DeckModel Model
        {
            get => _model;
        }


        readonly ObservableCollection<HeroViewModel> _heroes = new ObservableCollection<HeroViewModel>();
        public ObservableCollection<HeroViewModel> Heroes
        {
            get => _heroes;
        }

        const string DISPLAY_TITLE_PREFIX = "Squad ";
        string _displayTitle = $"{DISPLAY_TITLE_PREFIX}{double.NaN.ToString()}";
        public string DisplayTitle
        {
            get => _displayTitle;
            protected set => RASIC(ref _displayTitle, value);
        }


        public SquadViewModel(DeckModel model)
        {
            _model = model;
            DisplayTitle = $"{DISPLAY_TITLE_PREFIX}{_model.Slot.Value}";


            Heroes.CollectionChanged += Heroes_CollectionChanged;
            //EditModelHeroes = false;
            foreach (CreatureModelRefModel m in Model.Heroes.Sequence)
            {
                Heroes.Add(new HeroViewModel(m));
            }
            //EditModelHeroes = true;
            Model.Heroes.Sequence.CollectionChanged += Model_Heroes_CollectionChanged;
        }


        private void Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void Model_Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }
}
