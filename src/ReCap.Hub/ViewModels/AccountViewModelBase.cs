using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;
using ReCap.Hub.Models;

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


        public int CrogenitorLevel
        {
            get => Model.CrogenitorLevel.Value;
            set => Model.CrogenitorLevel.Value = value;
        }

        
        readonly ObservableCollection<HeroViewModel> _heroes = new ObservableCollection<HeroViewModel>();
        public ObservableCollection<HeroViewModel> Heroes
        {
            get => _heroes;
            //get => Model.Heroes.Sequence;
            /*protected set
            {
                if (_heroes != null)
                    _heroes.CollectionChanged -= Heroes_CollectionChanged;
                RASIC(ref _heroes, value);

                if (value != null)
                    value.CollectionChanged += Heroes_CollectionChanged;
            }*/
        }

        //protected abstract void Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);

        AccountModel _model = null;
        protected AccountModel Model
        {
            get => _model;
        }

        public void UpdateUserDisplayName(string newName)
        {
            Model.UserName.Value = newName;
            Model.SaveToXml();
        }
        public AccountViewModelBase(AccountModel model)
        {
            _model = model;
            Heroes.CollectionChanged += Heroes_CollectionChanged;
            EditModelHeroes = false;
            foreach (CreatureModel m in Model.Heroes.Sequence)
            {
                Heroes.Add(new HeroViewModel(m));
            }
            EditModelHeroes = true;
            Model.Heroes.Sequence.CollectionChanged += Model_Heroes_CollectionChanged;

            /*Model.Heroes.PropertyChanging += (s, e) => RefreshHeroes(true);
            
            ObservableCollection<HeroViewModel> oldHeroes = Model.Heroes.Sequence;
            Model.Heroes.PropertyChanged += (s, e) =>
            {
                if (oldHeroes != null)
                    oldHeroes.CollectionChanged -= Heroes_CollectionChanged;
                
                var newHeroes = Model.Heroes.Sequence;
                RASIC<ObservableCollection<HeroViewModel>>(ref oldHeroes, newHeroes, nameof(Heroes));
                
                if (newHeroes != null)
                    newHeroes.CollectionChanged += Heroes_CollectionChanged;
            };*/
        }

        bool _editModelHeroes = true;
        protected bool EditModelHeroes
        {
            get => _editModelHeroes;
            set => _editModelHeroes = value;
        }
        private void Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!EditModelHeroes)
                return;
            
            if (e.NewItems != null)
            {
                foreach (HeroViewModel hero in e.NewItems)
                {
                    Model.Heroes.Sequence.Add(hero.Model);
                }
            }

            if (e.OldItems != null)
            {
                foreach (HeroViewModel hero in e.OldItems)
                {
                    CreatureModel hModel = hero.Model;
                    foreach (CreatureModel model in Model.Heroes.Sequence)
                    {
                        if (hModel.NounID == model.NounID)
                        {
                            Model.Heroes.Sequence.Remove(model);
                            break;
                        }
                    }
                }
            }
        }

        private void Model_Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (EditModelHeroes)
                return;

            var act = e.Action;

            if (act == NotifyCollectionChangedAction.Reset)
            {
                Heroes.Clear();
                foreach (CreatureModel model in Model.Heroes.Sequence)
                {
                    Heroes.Add(new HeroViewModel(model));
                }
            }
            else
            {
                /*bool replaceOrMove = (act == NotifyCollectionChangedAction.Replace)
                        || (act == NotifyCollectionChangedAction.Move);*/


                /*if (
                            (act == NotifyCollectionChangedAction.Add)
                        || replaceOrMove
                    )*/
                if (
                        /*(act != NotifyCollectionChangedAction.Remove)
                        &&*/
                        (e.NewItems != null)
                    )
                {
                    foreach (CreatureModel model in e.NewItems)
                    {
                        Heroes.Add(new HeroViewModel(model));
                    }
                }


                /*if (
                            (act == NotifyCollectionChangedAction.Remove)
                        || replaceOrMove
                    )*/
                if (
                        /*(act != NotifyCollectionChangedAction.Add)
                        &&*/
                        (e.OldItems != null)
                    )
                {
                    foreach (CreatureModel model in e.OldItems)
                    {
                        foreach (HeroViewModel vm in Heroes)
                        {
                            if (vm.Model.NounID == model.NounID)
                            {
                                Heroes.Remove(vm);
                                break;
                            }
                        }
                    }
                }
            }
        }

        /*private void Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(Heroes));
        }

        void RefreshHeroes(bool changing)
        {
            string propName = nameof(Heroes);
            if (changing)
                this.RaisePropertyChanging(propName);
            else
                this.RaisePropertyChanged(propName);
        }*/

        public void Delete()
        {
            _model?.DeleteDocument();
            _model = null;
        }

        public void Rename(string newName)
        {
            Model?.Rename(newName);
        }
    }
}
