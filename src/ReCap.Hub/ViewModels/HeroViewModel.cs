using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.ViewModels
{
    public class HeroViewModel : ViewModelBase
    {
        public const string PNG_URL_PRE = @"http://127.0.0.1/game/service/png?template_id=";
        public const string PNG_URL_POST = @"&size=";
        public const string PNG_URL_POST_LARGE = @"large";

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

        public const string PNG_LARGE_URL_EL = "png_large_url";
        string _pngLargeUrl = null;
        public string PngLargeUrl
        {
            get => _pngLargeUrl;
            set => RASIC(ref _pngLargeUrl, value);
        }

        public const string PNG_SMALL_URL_EL = "png_small_url";
        string _pngSmallUrl = null;
        public string PngSmallUrl
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
        public const string STATS_ABILITY_KEYVALUES_EL = "stats_ability_keyvalues";
        public const string STATS_TEMPLATE_ABILITY_EL = "stats_template_ability";
        public const string STATS_TEMPLATE_ABILITY_KEYVALUES_EL = "stats_template_ability_keyvalues";

        public XElement ToXml()
        {
            XElement el = new XElement("creature");
            el.Add(
                new XElement(PNG_LARGE_URL_EL, PngLargeUrl)
                , new XElement(PNG_SMALL_URL_EL, PngSmallUrl)
                , new XElement(LEVEL_EL, Level)
                , new XElement(ITEM_POINTS_EL, ItemPoints)
                , new XElement(ID_EL, ID)
                , new XElement(NOUN_ID_EL, NounID)
                , new XElement(VERSION_EL, Version)
                , new XElement(STATS_EL, string.Empty)
                , new XElement(STATS_ABILITY_KEYVALUES_EL, string.Empty)
                , new XElement(STATS_TEMPLATE_ABILITY_EL, string.Empty)
                , new XElement(STATS_TEMPLATE_ABILITY_KEYVALUES_EL, string.Empty)
                );
            return el;
        }
    }
}
