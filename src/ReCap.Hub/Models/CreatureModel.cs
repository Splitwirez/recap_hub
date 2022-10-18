using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.Models
{
    public class CreatureModel : ModelBase, IXmlElementModel
    {
        public const string PNG_URL_PRE = @"http://127.0.0.1/game/service/png?template_id=";
        public const string PNG_URL_POST = @"&size=";
        public const string PNG_URL_POST_LARGE = @"large";

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
        public CreatureModel()
        {
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
            model.RefreshFromXml(el);
            return model;
        }

        public void RefreshFromXml(XElement element)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.Read(element);
            }
        }

        public void SaveToXml(ref XElement element)
        {
            foreach (var prop in _xmlProperties)
            {
                prop.Write(ref element);
            }
        }
    }
}