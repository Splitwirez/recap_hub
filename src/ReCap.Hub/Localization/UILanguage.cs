using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Localization
{
    public class UILanguage
        : ViewModelBase
    {
        public static readonly Assembly THIS_ASSEMBLY = typeof(UILanguage).Assembly;

        
        ResourceDictionary _strings = null;
        public ResourceDictionary Strings
        {
            get => _strings;
            private set => RASIC(ref _strings, value);
        }


        string _family = string.Empty;
        public string Family
        {
            get => _family;
            private set => RASIC(ref _family, value);
        }

        string _subTags = string.Empty;
        public string SubTags
        {
            get => _subTags;
            private set => RASIC(ref _subTags, value);
        }

        public string LanguageID
        {
            get => $"{Family}-{SubTags}";
        }

        static ResourceDictionary ConvertToResourceDictionary(Dictionary<object, string> dict)
        {
            ResourceDictionary ret = new();
            var keys = dict.Keys;
            foreach (var key in keys)
            {
                ret[key] = dict[key];
            }
            return ret;
        }
        public UILanguage(string langId, Dictionary<object, string> strings)
            : this(langId, 
                //stringResources.ToDictionary<object, string>((k, v) => new KeyValuePair<object, string>(k, v))
                ConvertToResourceDictionary(strings)
            )
        {}

        private UILanguage(string langId, ResourceDictionary strings)
        {
            Strings = strings;

            var tags = langId.Split('-');

            Family = tags[0];
            string subTags = tags[1];
            if (tags.Length > 2)
            {
                var exSubTags = tags.Skip(2).ToList();
                foreach (var exSubTag in exSubTags)
                {
                    subTags += $"-{exSubTag}";
                }
            }
            SubTags = subTags;
        }

        public static UILanguage Get(string langId)
        {
            string resDictStr = GetResourceString(THIS_ASSEMBLY, $"{UILanguageManager.LANG_DICT_PFX}{langId}");
            var resDict = AvaloniaRuntimeXamlLoader.Parse<ResourceDictionary>(resDictStr);
            return new(langId, resDict);
        }

        static string GetResourceString(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}