using Avalonia.Media.Imaging;
using ReCap.Hub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.ViewModels
{
    public class HeroViewModel : ViewModelBase
    {
        /*public const string PNG_URL_PRE = @"http://127.0.0.1/game/service/png?template_id=";
        public const string PNG_URL_POST = @"&size=";
        public const string PNG_URL_POST_LARGE = @"large";*/

        string _shortName = string.Empty;
        public string ShortName
        {
            get => _shortName;
            set => RASIC(ref _shortName, value);
        }


        string _loreTitle = string.Empty;
        public string LoreTitle
        {
            get => _loreTitle;
            set => RASIC(ref _loreTitle, value);
        }

        Bitmap _thumbnail = null;
        public Bitmap Thumbnail
        {
            get => _thumbnail;
            set => RASIC(ref _thumbnail, value);
        }

        /*public const string PNG_LARGE_URL_EL = "png_large_url";
        string _pngLargeUrl = null;
        public string PngLargeUrl
        {
            get => _pngLargeUrl;
            set => RASIC(ref _pngLargeUrl, value);
        }

        public const string PNG_THUMB_URL_EL = "png_thumb_url";
        string _pngSmallUrl = null;
        public string PngThumbUrl
        {
            get => _pngSmallUrl;
            set => RASIC(ref _pngSmallUrl, value);
        }

        public const string LEVEL_EL = "gear_score";
        double _level = 0;
        public double Level
        {
            get => _level;
            set => RASIC(ref _level, value);
        }

        public const string ITEM_POINTS_EL = "item_points";
        double _itemPoints = 0;
        public double ItemPoints
        {
            get => _itemPoints;
            set => RASIC(ref _itemPoints, value);
        }

        public const string ID_EL = "id";
        int _id = 0;
        public int ID
        {
            get => _id;
            set => RASIC(ref _id, value);
        }

        public const string NOUN_ID_EL = "noun_id";
        string _nounId = string.Empty;
        public string NounID
        {
            get => _nounId;
            set => RASIC(ref _nounId, value);
        }

        public const string VERSION_EL = "version";
        int _version = 0;
        public int Version
        {
            get => _version;
            set => RASIC(ref _version, value);
        }

        public const string STATS_EL = "stats";
        string _stats = string.Empty;
        public string Stats
        {
            get => _stats;
            set => RASIC(ref _stats, value);
        }

        public const string STATS_ABILITY_KEYVALUES_EL = "stats_ability_keyvalues";
        string _statsAbilityKeyValues = string.Empty;
        public string StatsAbilityKeyValues
        {
            get => _statsAbilityKeyValues;
            set => RASIC(ref _statsAbilityKeyValues, value);
        }
        
        public const string STATS_TEMPLATE_ABILITY_EL = "stats_template_ability";
        string _statsTemplateAbility = string.Empty;
        public string StatsTemplateAbility
        {
            get => _statsTemplateAbility;
            set => RASIC(ref _statsTemplateAbility, value);
        }
        
        public const string STATS_TEMPLATE_ABILITY_KEYVALUES_EL = "stats_template_ability_keyvalues";
        string _statsTemplateAbilityKeyValues = string.Empty;
        public string StatsTemplateAbilityKeyValues
        {
            get => _statsTemplateAbilityKeyValues;
            set => RASIC(ref _statsTemplateAbilityKeyValues, value);
        }
        
        

        public XElement ToXml()
        {
            List<XElement> elements = new List<XElement>()
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
            return _element;
        }

        XElement _element = null;
        public static HeroViewModel FromXml(XElement el)
        {
            var hero = new HeroViewModel();
            
            if (el.TryGetElementValue(PNG_LARGE_URL_EL, out string pngLargeUrl))
                hero.PngLargeUrl = pngLargeUrl;

            if (el.TryGetElementValue(PNG_THUMB_URL_EL, out string pngThumbUrl))
                hero.PngThumbUrl = pngThumbUrl;

            if (el.TryGetElementValue(NOUN_ID_EL, out string nounID))
                hero.NounID = el.Element(NOUN_ID_EL).Value;
            
            if (el.TryGetElementValue(LEVEL_EL, out string levelStr) && double.TryParse(levelStr, out double level))
                hero.Level = level;

            if (el.TryGetElementValue(ITEM_POINTS_EL, out string itemPointsStr) && double.TryParse(itemPointsStr, out double itemPoints))
                hero.ItemPoints = itemPoints;

            if (el.TryGetElementValue(ID_EL, out string idStr) && int.TryParse(idStr, out int id))
                hero.ID = id;

            if (el.TryGetElementValue(VERSION_EL, out string versionStr) && int.TryParse(versionStr, out int version))
                hero.Version = version;

            if (el.TryGetElementValue(STATS_EL, out string stats))
                hero.Stats = stats;
            
            if (el.TryGetElementValue(STATS_ABILITY_KEYVALUES_EL, out string statsAbilityKeyValues))
                hero.StatsAbilityKeyValues = statsAbilityKeyValues;
            
            if (el.TryGetElementValue(STATS_TEMPLATE_ABILITY_EL, out string statsTemplateAbility))
                hero.StatsTemplateAbility = statsTemplateAbility;
            
            if (el.TryGetElementValue(STATS_TEMPLATE_ABILITY_KEYVALUES_EL, out string statsTemplateAbilityKeyValues))
                hero.StatsTemplateAbilityKeyValues = statsTemplateAbilityKeyValues;
            
                /*= el.Element(STATS_EL, string.Empty)
                = el.Element(STATS_ABILITY_KEYVALUES_EL, string.Empty)
                = el.Element(STATS_TEMPLATE_ABILITY_EL, string.Empty)
                = el.Element(STATS_TEMPLATE_ABILITY_KEYVALUES_EL, string.Empty)* /
            hero._element = el;
            return hero;
        }*/


        readonly CreatureModel _model = null;
        public CreatureModel Model
        {
            get => _model;
        }
        public HeroViewModel(CreatureModel model)
        {
            _model = model;
            ShortName = _model.ID.Value.ToString();
            LoreTitle = _model.NounID.Value;
        }
    }
}
