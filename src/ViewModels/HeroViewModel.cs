using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class HeroViewModel : ViewModelBase
    {
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

        int _level = 0;
        public int Level
        {
            get => _level;
            set => RASIC(ref _level, value);
        }
    }
}
