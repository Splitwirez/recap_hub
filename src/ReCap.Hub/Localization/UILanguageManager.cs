using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Localization
{
    public partial class UILanguageManager
        : ViewModelBase
    {
        // Must match $(LSvgPfx) in 'ReCap.Hub.csproj'
        public const string LANG_DICT_PFX = "uilang:";


        public static readonly string DEFAULT_LANG_ID = "en-ca";
        public static readonly UILanguageManager Instance = new();

        
        readonly ResourceDictionary _dictionary;
        public ResourceDictionary Dictionary
        {
            get => _dictionary;
        }

        List<string> _languageIDs = new List<string>();
        ObservableCollection<UILanguage> _languages = new();
        public ObservableCollection<UILanguage> Languages
        {
            get => _languages;
        }

        UILanguage _currentLanguage;
        public UILanguage CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                var newCurrentLang = RASIC(ref _currentLanguage, value);
                if (newCurrentLang == null)
                    return;
                if (newCurrentLang == _defaultLanguage)
                    return;
                
                var mergedDictsCount = Dictionary.MergedDictionaries.Count;
                if (mergedDictsCount > 1)
                {
                    for (int i = 1; i < mergedDictsCount; i++)
                    {
                        Dictionary.MergedDictionaries.RemoveAt(1);
                    }
                }
                Dictionary.MergedDictionaries.Add(newCurrentLang.Strings);
            }
        }

        static UILanguage _defaultLanguage2 = null;
        public static UILanguage DefaultLanguage
        {
            get => _defaultLanguage2;
        }
        
        readonly UILanguage _defaultLanguage;
        private UILanguageManager()
        {
            var langResPaths = UILanguage.THIS_ASSEMBLY
                .GetManifestResourceNames()
                .Where(x => x.StartsWith(LANG_DICT_PFX))
                .ToList()
            ;
            
            foreach (string langResPath in langResPaths)
            {
                string langID = langResPath.Substring(LANG_DICT_PFX.Length);
                UILanguage lang = UILanguage.Get(langID);
                Languages.Add(lang);
                _languageIDs.Add(langID);
            }


            _defaultLanguage = Languages.First(x => x.LanguageID == DEFAULT_LANG_ID);
            if (_defaultLanguage2 == null)
                _defaultLanguage2 = _defaultLanguage;


#if DEBUG
            int targetTestLanguage = 0;
            if (targetTestLanguage > 0)
            {
                var testLanguages = LocalizationTest.TestLanguages;
                if (testLanguages.Count >= 1)
                {
                    foreach (var testLang in testLanguages)
                    {
                        Languages.Add(testLang);
                    }
                    _defaultLanguage = testLanguages[targetTestLanguage - 1];
                }
            }
#endif


            _dictionary = new()
            {
                MergedDictionaries = 
                {
                    _defaultLanguage.Strings
                }
            };

            
            
            var currentLangID = CultureInfo.CurrentUICulture.Name.ToLowerInvariant();
            
            
            CurrentLanguage = TryGetRoundedLanguage(currentLangID, out UILanguage matchedLang)
                ? matchedLang
                : _defaultLanguage
            ;
        }


        readonly ReadOnlyCollection<string> _LANGUAGE_ROUNDING_ALLOWED_GROUPS
            = new List<string>()
        {
            "en",
            "es",
            "ca"
        }.AsReadOnly();

        bool TryGetRoundedLanguage(string langID, out UILanguage roundedLanguage)
        {
            roundedLanguage = null;
            if (langID.StartsWith(LocalizationTest.TEST_LANG_PREFIX))
                return false;
            if (!TryGetRoundedLanguageIdentifier(langID, out string roundedLangID))
                return false;
            
            roundedLanguage = Languages.FirstOrDefault(x => IsSameLangID(x.LanguageID, roundedLangID));
            return roundedLanguage != default;
        }
        bool TryGetRoundedLanguageIdentifier(string langID, out string roundedLangID)
        {
            var target = _languageIDs.FirstOrDefault(x => IsSameLangID(x, langID));
            
            if (target != default)
            {
                string langGroup = GetLangGroup(langID);
                if (_LANGUAGE_ROUNDING_ALLOWED_GROUPS.Contains(langGroup)) //_LANGUAGE_ROUNDING_ALLOWED_GROUPS.Any(x => IsSameLangID(x, langGroup)))
                {
                    target = _languageIDs.FirstOrDefault(x => IsSameLangID(GetLangGroup(x), langID));
                }
            }

            if (target != default)
            {
                roundedLangID = target;
                return roundedLangID != null;
            }
            roundedLangID = null;
            return false;
        }

        public string GetLocalizedText(string key, string fallback)
        {
            string fallbackText = $"{fallback} (MISSING FROM LANGUAGE)";
            if (!App.Current.TryFindResource(key, out object localizedTextRes))
                return fallbackText;
            
            
            if (localizedTextRes is string localizedText)
            {
                return localizedText;
            }
            else
            {
                return fallbackText;
            }
        }

        public static bool IsSameLangID(string langID1, string langID2)
            => langID1 == null
                ? langID1 == langID2
                : langID1.Equals(langID2, StringComparison.OrdinalIgnoreCase)
            ;
        
        public static string GetLangGroup(string langID)
            => langID.Contains('-')
                ? langID.Split('-')[0]
                : langID
        ;
    }
}