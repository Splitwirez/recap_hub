using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.Models
{
    public abstract class CreatureModelBase : ModelBase, ICanIntoXml
    {
        public const string PNG_URL_PRE = @"http://127.0.0.1/game/service/png?template_id=";
        public const string PNG_URL_POST = @"&size=";
        public const string PNG_URL_POST_LARGE = @"large";

        /*public const string PNG_LARGE_URL_EL = "png_large_url";
        readonly XmlStringProperty _PngLargeUrl = new(string.Empty, PNG_LARGE_URL_EL);
        public string PngLargeUrl
        {
            get => _PngLargeUrl.Value;
            set => this.XmlRASIC(_PngLargeUrl, value);
        }

        public const string PNG_THUMB_URL_EL = "png_thumb_url";
        readonly XmlStringProperty _PngThumbUrl = new(string.Empty, PNG_THUMB_URL_EL);
        public string PngThumbUrl
        {
            get => _PngThumbUrl.Value;
            set => this.XmlRASIC(_PngThumbUrl, value);
        }

        public const string LEVEL_EL = "gear_score";
        readonly XmlProperty<double> _Level = new(0, LEVEL_EL);
        public double Level
        {
            get => _Level.Value;
            set => this.XmlRASIC(_Level, value);
        }

        public const string ITEM_POINTS_EL = "item_points";
        readonly XmlProperty<double> _ItemPoints = new(0, ITEM_POINTS_EL);
        public double ItemPoints
        {
            get => _ItemPoints.Value;
            set => this.XmlRASIC(_ItemPoints, value);
        }

        public const string ID_EL = "id";
        readonly XmlProperty<int> _ID = new(0, ID_EL);
        public int ID
        {
            get => _ID.Value;
            set => this.XmlRASIC(_ID, value);
        }

        public const string NOUN_ID_EL = "noun_id";
        readonly XmlStringProperty _NounID = new(string.Empty, NOUN_ID_EL);
        public string NounID
        {
            get => _NounID.Value;
            set => this.XmlRASIC(_NounID, value);
        }

        public const string VERSION_EL = "version";
        readonly XmlProperty<int> _Version = new(0, VERSION_EL);
        public int Version
        {
            get => _Version.Value;
            set => this.XmlRASIC(_Version, value);
        }

        public const string STATS_EL = "stats";
        readonly XmlStringProperty _Stats = new(string.Empty, STATS_EL);
        public string Stats
        {
            get => _Stats.Value;
            set => this.XmlRASIC(_Stats, value);
        }

        public const string STATS_ABILITY_KEYVALUES_EL = "stats_ability_keyvalues";
        readonly XmlStringProperty _StatsAbilityKeyValues = new(string.Empty, STATS_ABILITY_KEYVALUES_EL);
        public string StatsAbilityKeyValues
        {
            get => _StatsAbilityKeyValues.Value;
            set => this.XmlRASIC(_StatsAbilityKeyValues, value);
        }

        public const string STATS_TEMPLATE_ABILITY_EL = "stats_template_ability";
        readonly XmlStringProperty _StatsTemplateAbility = new(string.Empty, STATS_TEMPLATE_ABILITY_EL);
        public string StatsTemplateAbility
        {
            get => _StatsTemplateAbility.Value;
            set => this.XmlRASIC(_StatsTemplateAbility, value);
        }

        public const string STATS_TEMPLATE_ABILITY_KEYVALUES_EL = "stats_template_ability_keyvalues";
        readonly XmlStringProperty _StatsTemplateAbilityKeyValues = new(string.Empty, STATS_TEMPLATE_ABILITY_KEYVALUES_EL);
        public string StatsTemplateAbilityKeyValues
        {
            get => _StatsTemplateAbilityKeyValues.Value;
            set => this.XmlRASIC(_StatsTemplateAbilityKeyValues, value);
        }*/
        

        public const string PNG_LARGE_URL_EL = "png_large_url";
        public readonly XmlStringProperty PngLargeUrl = new(string.Empty, PNG_LARGE_URL_EL);

        public const string PNG_THUMB_URL_EL = "png_thumb_url";
        public readonly XmlStringProperty PngThumbUrl = new(string.Empty, PNG_THUMB_URL_EL);

        public const string LEVEL_EL = "gear_score";
        public readonly XmlProperty<double> Level = new(0, LEVEL_EL);

        public const string ITEM_POINTS_EL = "item_points";
        public readonly XmlProperty<double> ItemPoints = new(0, ITEM_POINTS_EL);

        public const string ID_EL = "id";
        public readonly XmlProperty<int> ID = new(0, ID_EL);

        public const string NOUN_ID_EL = "noun_id";
        public readonly XmlStringProperty NounID = new(string.Empty, NOUN_ID_EL);

        public const string VERSION_EL = "version";
        public readonly XmlProperty<int> Version = new(0, VERSION_EL);

        public const string STATS_EL = "stats";
        public readonly XmlStringProperty Stats = new(string.Empty, STATS_EL);

        public const string STATS_ABILITY_KEYVALUES_EL = "stats_ability_keyvalues";
        public readonly XmlStringProperty StatsAbilityKeyValues = new(string.Empty, STATS_ABILITY_KEYVALUES_EL);

        public const string STATS_TEMPLATE_ABILITY_EL = "stats_template_ability";
        public readonly XmlStringProperty StatsTemplateAbility = new(string.Empty, STATS_TEMPLATE_ABILITY_EL);

        public const string STATS_TEMPLATE_ABILITY_KEYVALUES_EL = "stats_template_ability_keyvalues";
        public readonly XmlStringProperty StatsTemplateAbilityKeyValues = new(string.Empty, STATS_TEMPLATE_ABILITY_KEYVALUES_EL);



        public XElement ToXml()
        {
            XElement creatureEl = new XElement("creature");
            SaveToXml(ref creatureEl);
            return creatureEl;
            /*List<XElement> elements = new List<XElement>()
            {
                new XElement(PNG_LARGE_URL_EL, PngLargeUrl)
                , new XElement(PNG_THUMB_URL_EL, PngThumbUrl)
                , new XElement(LEVEL_EL, Level)
                , new XElement(ITEM_POINTS_EL, ItemPoints)
                , new XElement(ID_EL, ID)
                , new XElement(NOUN_ID_EL, NounID)
                , new XElement(VERSION_EL, Version)
                , new XElement(STATS_EL, Stats)
                , new XElement(STATS_ABILITY_KEYVALUES_EL, StatsAbilityKeyValues)
                , new XElement(STATS_TEMPLATE_ABILITY_EL, StatsTemplateAbility)
                , new XElement(STATS_TEMPLATE_ABILITY_KEYVALUES_EL, StatsTemplateAbilityKeyValues)
            };
            if (_element != null)
            {
                foreach (var newEl in elements)
                {
                    var oldEl = _element.Element(newEl.Name);
                    if (oldEl != null)
                        oldEl.Value = newEl.Value;
                    else
                        _element.Add(newEl);
                }
            }
            else
            {
                XElement el = new XElement("creature");
                foreach (var newEl in elements)
                {
                    el.Add(newEl);
                }
                _element = el;
            }
            return _element;*/
        }

        XElement _element = null;

        List<XmlPropertyBase> _xmlProperties = new List<XmlPropertyBase>();
        public CreatureModelBase()
        {
            /*_xmlProperties.Add(_PngLargeUrl);
            _xmlProperties.Add(_PngThumbUrl);
            _xmlProperties.Add(_Level);
            _xmlProperties.Add(_ItemPoints);
            _xmlProperties.Add(_ID);
            _xmlProperties.Add(_NounID);
            _xmlProperties.Add(_Version);
            _xmlProperties.Add(_Stats);
            _xmlProperties.Add(_StatsAbilityKeyValues);
            _xmlProperties.Add(_StatsTemplateAbility);
            _xmlProperties.Add(_StatsTemplateAbilityKeyValues);*/
            _xmlProperties.Add(PngLargeUrl);
            _xmlProperties.Add(PngThumbUrl);
            _xmlProperties.Add(Level);
            _xmlProperties.Add(ItemPoints);
            _xmlProperties.Add(ID);
            _xmlProperties.Add(NounID);
            _xmlProperties.Add(Version);
            _xmlProperties.Add(Stats);
            _xmlProperties.Add(StatsAbilityKeyValues);
            _xmlProperties.Add(StatsTemplateAbility);
            _xmlProperties.Add(StatsTemplateAbilityKeyValues);
        }
        
        public static CreatureModel FromXml(XElement el)
        {
            var model = new CreatureModel();
            model.RefreshFromXml(el, false);
            return model;
        }

        public void RefreshFromXml(XElement element)
            => RefreshFromXml(element, true);
        public virtual void RefreshFromXml(XElement element, bool notify)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.ReadFromXml(element, notify);
            }
        }

        public virtual void SaveToXml(ref XElement element)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.WriteToXml(ref element);
            }
        }

        readonly string _xmlElementTagName = "creature";
        public string XmlElementTagName
        {
            get => _xmlElementTagName;
        }
    }
}