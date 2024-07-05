using System;
using System.Collections.Generic;

namespace ReCap.Hub.Localization
{
    public static partial class LocalizationTest
    {
        public const string TEST_LANG_PREFIX = "qrecap-";
        
        public static readonly IReadOnlyList<UILanguage> TestLanguages;
        static LocalizationTest()
        {
            //GenPseudoChars();
            var keys = Keys.KeyHelper.AllKeys;
            TestLanguages = new List<UILanguage>()
            {
                CreatePseudoLanguage(keys),
                CreateKeyTestLanguage(keys),
            };
        }

        static UILanguage CreateKeyTestLanguage(IEnumerable<string> keys)
        {
            Dictionary<object, string> strings = new();
            foreach (var key in keys)
            {
                strings.Add(key, $"localized[key]");
            }
            return new UILanguage($"{TEST_LANG_PREFIX}keystest", strings);
        }


        static UILanguage CreatePseudoLanguage(IEnumerable<string> keys)
        {
            var defaultLang = UILanguageManager.DefaultLanguage;
            var defaultStrings = defaultLang.Strings;

            Dictionary<object, string> strings = new();
            foreach (var key in keys)
            {
                string value = defaultStrings.ContainsKey(key)
                    ? (string)(defaultStrings[key])
                    : $"localized[key]"
                ;
                value = PseudoLocalize(value);
                strings.Add(key, value);
            }
            return new UILanguage($"{TEST_LANG_PREFIX}ploc", strings);
        }



        static readonly Random _RANDOM = new();
        static string PseudoLocalize(string input)
        {
            string generated = string.Empty;
            
            int length = input.Length;
            int rand = _RANDOM.Next(9);
            for (int i = 0; i < length; i++)
            {
                var ch = input[i];
                rand = _RANDOM.Next(9);
                
                if (!_PSEUDO_CHARS.TryGetValue(ch, out string[] alts))
                {
                    generated += ch;
                    continue;
                }

                
                bool insertAltChar = true;
                if (rand > 2)
                {
                    generated += ch;
                    insertAltChar = false;
                }
                
                if (!insertAltChar)
                {
                    rand = _RANDOM.Next(9);
                    insertAltChar = rand < 6;
                }
                
                if (insertAltChar)
                {
                    int altsCount = alts.Length;
                    int altIndex = _RANDOM.Next(altsCount);
                    generated += alts[altIndex];
                }
            }
            if (rand > 7)
                generated += "_";
            
            return $"{generated}";
        }
        static readonly IReadOnlyDictionary<char, string[]> _PSEUDO_CHARS = new Dictionary<char, string[]>()
        {
            {
                'A',
                new[] { "À", "Á", "Â", "Ã", "Ä", "Å", "Ǻ", "Ā", "Ă", "Ą", "Ǎ", "Α", "Ά", "Ả", "Ạ", "Ầ", "Ẫ", "Ẩ", "Ậ", "Ằ", "Ắ", "Ẵ", "Ẳ", "Ặ", "А", } 
            },
            {
                'a',
                new[] { "à", "á", "â", "ã", "å", "ǻ", "ā", "ă", "ą", "ǎ", "ª", "α", "ά", "ả", "ạ", "ầ", "ấ", "ẫ", "ẩ", "ậ", "ằ", "ắ", "ẵ", "ẳ", "ặ", "а", } 
            },
            {
                'B',
                new[] { "β", "Б", } 
            },
            {
                'b',
                new[] { "Ъ", "ъ", "Ь", "ь", "б", } 
            },
            {
                'C',
                new[] { "Ç", "Ć", "Ĉ", "Ċ", "Č", } 
            },
            {
                'c',
                new[] { "ç", "ć", "ĉ", "ċ", "č", } 
            },
            {
                'D',
                new[] { "Д", } 
            },
            {
                'd',
                new[] { "д", } 
            },
            {
                'E',
                new[] { "È", "É", "Ê", "Ë", "Ē", "Ĕ", "Ė", "Ę", "Ě", "Ε", "Έ", "Ẽ", "Ẻ", "Ẹ", "Ề", "Ế", "Ễ", "Ể", "Ệ", "Е", "Э", } 
            },
            {
                'e',
                new[] { "è", "é", "ê", "ë", "ē", "ĕ", "ė", "ę", "ě", "έ", "ε", "ẽ", "ẻ", "ẹ", "ề", "ế", "ễ", "ể", "ệ", "е", "э", } 
            },
            {
                'F',
                new[] { "Ф", } 
            },
            {
                'f',
                new[] { "ф", "ƒ" } 
            },
            {
                'G',
                new[] { "Ĝ", "Ğ", "Ġ", "Ģ", "Γ", "Г", "Ґ", } 
            },
            {
                'g',
                new[] { "ĝ", "ğ", "ġ", "ģ", "γ", "г", "ґ", } 
            },
            {
                'H',
                new[] { "Ĥ", "Ħ", } 
            },
            {
                'h',
                new[] { "ĥ", "ħ", } 
            },
            {
                'I',
                new[] { "Ì", "Í", "Î", "Ï", "Ĩ", "Ī", "Ĭ", "Ǐ", "Į", "İ", "Η", "Ή", "Ί", "Ι", "Ϊ", "Ỉ", "Ị", "И", "Ы", } 
            },
            {
                'i',
                new[] { "ì", "í", "î", "ï", "ĩ", "ī", "ĭ", "ǐ", "į", "ı", "η", "ή", "ί", "ι", "ϊ", "ỉ", "ị", "и", "ы", "ї", } 
            },
            {
                'J',
                new[] { "Ĵ", } 
            },
            {
                'j',
                new[] { "ĵ", } 
            },
            {
                'K',
                new[] { "Ķ", "Κ", "К", } 
            },
            {
                'k',
                new[] { "ķ", "κ", "к", } 
            },
            {
                'L',
                new[] { "Ĺ", "Ļ", "Ľ", "Ŀ", "Ł", "Λ", "Л", } 
            },
            {
                'l',
                new[] { "ĺ", "ļ", "ľ", "ŀ", "ł", "λ", "л", } 
            },
            {
                'M',
                new[] { "М", } 
            },
            {
                'm',
                new[] { "м", } 
            },
            {
                'N',
                new[] { "Ñ", "Ń", "Ņ", "Ň", "Ν", "Н", } 
            },
            {
                'n',
                new[] { "ñ", "ń", "ņ", "ň", "ŉ", "н", } 
            },
            {
                'O',
                new[] { "Ò", "Ó", "Ô", "Õ", "Ō", "Ŏ", "Ǒ", "Ő", "Ơ", "Ø", "Ǿ", "Ο", "Ό", "Ω", "Ώ", "Ỏ", "Ọ", "Ồ", "Ố", "Ỗ", "Ổ", "Ộ", "Ờ", "Ớ", "Ỡ", "Ở", "Ợ", "О", } 
            },
            {
                'o',
                new[] { "ò", "ó", "ô", "õ", "ō", "ŏ", "ǒ", "ő", "ơ", "ø", "ǿ", "º", "ο", "ό", "ω", "ώ", "ỏ", "ọ", "ồ", "ố", "ỗ", "ổ", "ộ", "ờ", "ớ", "ỡ", "ở", "ợ", "о", } 
            },
            {
                'P',
                new[] { "П", } 
            },
            {
                'p',
                new[] { "π", "п", } 
            },
            {
                'R',
                new[] { "Ŕ", "Ŗ", "Ř", "Ρ", "Р", } 
            },
            {
                'r',
                new[] { "ŕ", "ŗ", "ř", "ρ", "р", } 
            },
            {
                'S',
                new[] { "Ś", "Ŝ", "Ş", "Ș", "Š", "Σ", "С", } 
            },
            {
                's',
                new[] { "ś", "ŝ", "ş", "ș", "š", "ſ", "σ", "ς", "с", } 
            },
            {
                'T',
                new[] { "Ț", "Ţ", "Ť", "Ŧ", "τ", "Т", } 
            },
            {
                't',
                new[] { "ț", "ţ", "ť", "ŧ", "т", } 
            },
            {
                'U',
                new[] { "Ù", "Ú", "Û", "Ũ", "Ū", "Ŭ", "Ů", "Ű", "Ų", "Ư", "Ǔ", "Ǖ", "Ǘ", "Ǚ", "Ǜ", "Ũ", "Ủ", "Ụ", "Ừ", "Ứ", "Ữ", "Ử", "Ự", "У", } 
            },
            {
                'u',
                new[] { "ù", "ú", "û", "ũ", "ū", "ŭ", "ů", "ű", "ų", "ư", "ǔ", "ǖ", "ǘ", "ǚ", "ǜ", "υ", "ύ", "ϋ", "ủ", "ụ", "ừ", "ứ", "ữ", "ử", "ự", "у", } 
            },
            {
                'Y',
                new[] { "Ý", "Ÿ", "Ŷ", "Υ", "Ύ", "Ϋ", "Ỳ", "Ỹ", "Ỷ", "Ỵ", "Й", } 
            },
            {
                'y',
                new[] { "ý", "ÿ", "ŷ", "ỳ", "ỹ", "ỷ", "ỵ", "й", } 
            },
            {
                'V',
                new[] { "В", } 
            },
            {
                'v',
                new[] { "ν", "μ", "в", } 
            },
            {
                'W',
                new[] { "Ŵ", } 
            },
            {
                'w',
                new[] { "ŵ", } 
            },
            {
                'Z',
                new[] { "Ź", "Ż", "Ž", "Ζ", "З", } 
            },
            {
                'z',
                new[] { "ź", "ż", "ž", "ζ", "з", } 
            },
        };
    }
}