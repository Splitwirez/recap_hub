using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReCap.Hub.Localization.Keys
{
    public static class Language
    {
        public static readonly string DisplayName = default;
        static Language()
        {
            KeyHelper.PrepareKeys(typeof(Language));
        }
    }
    public static class App
    {
        public static readonly string DisplayName = default;
        static App()
        {
            KeyHelper.PrepareKeys(typeof(App));
        }
    }
    public static class HomePage
    {
        public static readonly string OfflinePlayTabHeader = default;
        public static readonly string OnlinePlayTabHeader = default;
        public static readonly string ShowPreferences = default;
        static HomePage()
        {
            KeyHelper.PrepareKeys(typeof(HomePage));
        }
    }
    public static class LocateGame
    {
        public static readonly string Title = default;
        public static readonly string ShortDescription = default;
        public static readonly string Instruction = default;

        public static readonly string ChooseDetectByLaunch = default;
        public static readonly string ChooseBrowseForGamePath = default;
        static LocateGame()
        {
            KeyHelper.PrepareKeys(typeof(LocateGame));
        }
    }

    public static class LocalPlay
    {
        public static readonly string GameConfigsHeader = default;
        public static readonly string ShowGameConfigs = default;
        public static readonly string HideGameConfigs = default;
        static LocalPlay()
        {
            KeyHelper.PrepareKeys(typeof(LocalPlay));
        }
    }

    public static class AccountView
    {
        public static readonly string CrogenitorLevel = default;
        static AccountView()
        {
            KeyHelper.PrepareKeys(typeof(AccountView));
        }
    }
        
    internal static class KeyHelper
    {
        static readonly Type _TARGET_FIELD_TYPE = typeof(string);
        public static void PrepareKeys(Type type)
        {
            string typeName = type.Name;
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach(FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.FieldType != _TARGET_FIELD_TYPE)
                    continue;
                
                string key = typeName + '.' + fieldInfo.Name;
                fieldInfo.SetValue(null, key);
                
                _allKeys.Add(key);
            }
        }

        static List<string> _allKeys = new();
        public static IEnumerable<string> AllKeys
        {
            get => _allKeys;
        }
    }
}