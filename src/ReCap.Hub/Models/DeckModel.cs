using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.Models
{
    public class DeckModel : ModelBase, ICanIntoXml
    {
        const string NAME_EL = "name";
        public readonly XmlStringProperty Name = new(string.Empty, NAME_EL);

        const string ID_EL = "id";
        public readonly XmlStringProperty ID = new(string.Empty, ID_EL);

        const string CATEGORY_EL = "category";
        public readonly XmlStringProperty Category = new(string.Empty, CATEGORY_EL);

        const string SLOT_EL = "slot";
        public readonly XmlStringProperty Slot = new(string.Empty, SLOT_EL);

        const string LOCKED_EL = "locked";
        public readonly XmlNumericalBoolProperty IsLocked = new(false, LOCKED_EL);

        const string HEROES_EL = "creatures";
        bool _heroesPopulated = false;
        public readonly XmlICanIntoXmlElementsProperty<CreatureModelRefModel> Heroes = new (HEROES_EL);
        /*public readonly XmlElementsProperty<XElement> HeroesRaw = new(
            (el) => el,
            (el) => el,
            HEROES_EL);*/

        /*readonly ObservableCollection<CreatureModel> _heroes = new ObservableCollection<CreatureModel>();
        public ObservableCollection<CreatureModel> Heroes
        {
            get => _heroes;
        }*/
        //public readonly XmICanIntoXmlElementsProperty<CreatureModel> Heroes = new(HEROES_EL);
        /*public readonly XmlElementsProperty<CreatureModel> Heroes = new(
            (XElement inVal) => CreatureModel.FromXml(inVal),
            (CreatureModel data) => data.ToXml(),
            HEROES_EL);*/


        readonly List<XmlPropertyBase> _xmlProperties = new List<XmlPropertyBase>();
        public DeckModel()
            : this(1)
        {
        }

        private DeckModel(byte index)
        {
            _xmlProperties.Add(Name);
            _xmlProperties.Add(ID);
            _xmlProperties.Add(Category);
            _xmlProperties.Add(Slot);
            _xmlProperties.Add(IsLocked);
            _xmlProperties.Add(Heroes);
        }

        public DeckModel(byte index, params CreatureModel[] heroes)
            : this(index)
        {
            string i = index.ToString();
            Name.Value = $"Slot {i}";
            ID.Value = i;
            Slot.Value = i;


            if (heroes == null)
                return;
            if (heroes.Length > 0)
            {
                if (heroes.Length > 3)
                    throw new Exception("Squads can only have up to three heroes!");

                PopulateHeroes(heroes);
            }
        }

        public void PopulateHeroes(IEnumerable<CreatureModel> heroes)
        {
            if (_heroesPopulated)
                return;

            foreach (var hero in heroes)
            {
                /*if (hero is CreatureModelRefModel heroRef)
                    Heroes.Sequence.Add(heroRef);
                else*/
                    Heroes.Sequence.Add(new CreatureModelRefModel(hero));
            }
            _heroesPopulated = true;
        }

        public void EnsureHeroes(IEnumerable<CreatureModel> heroes, bool onlyIfNull)
        {
            //IEnumerable<CreatureModelRefModel> heroRefs = Heroes.Sequence;
            if (Heroes.Sequence.Count > 0)
            {
                //heroRefs = heroRefs.Where(x => x.Creature == null);

                foreach (var heroRef in Heroes.Sequence)
                {
                    if (onlyIfNull && (heroRef.Creature != null))
                        continue;

                    heroRef.TryEnsureCreature(heroes);
                }
            }
        }

        public DeckModel(ref XElement element)
            : this()
        {
            RefreshFromXml(element);
        }


        bool _firstRefresh = true;
        public void RefreshFromXml(XElement element)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.ReadFromXml(element, true);
                /*if (_firstRefresh)
                    prop.PropertyChanged += WhenXmlPropertyChanged;*/
            }
            _firstRefresh = false;
        }

        public void SaveToXml(ref XElement element)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.WriteToXml(ref element);
            }
        }

        readonly string _xmlElementTagName = "deck";
        public string XmlElementTagName
        {
            get => _xmlElementTagName;
        }
    }
}
