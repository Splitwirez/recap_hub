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
    public static class Globals
    {
        public static readonly string OK = default;
        public static readonly string Accept = default;
        public static readonly string Cancel = default;
        public static readonly string Yes = default;
        public static readonly string No = default;
        public static readonly string Play = default;
        public static readonly string Delete = default;
        public static readonly string Browse = default;
        static Globals()
        {
            KeyHelper.PrepareKeys(typeof(Globals));
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
        
        public static readonly string DarksporeExecutablePath = default;
        public static readonly string WINEPREFIXPath = default;
        public static readonly string WINEExecutablePath = default;
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
        public static readonly string SaveGamesHeader = default;
        public static readonly string CreateNewSaveGame = default;
        public static readonly string RenameSaveGame = default;
        public static readonly string SaveGameNameHeader = default;
        static LocalPlay()
        {
            KeyHelper.PrepareKeys(typeof(LocalPlay));
        }
    }

    public static class OnlinePlay
    {
        public static readonly string ServersHeader = default;
        static OnlinePlay()
        {
            KeyHelper.PrepareKeys(typeof(OnlinePlay));
        }
    }

    public static class AccountView
    {
        public static readonly string CrogenitorLevel = default;
        public static readonly string CurrentThreat = default;
        public static readonly string SquadsHeader = default;
        public static readonly string HeroesHeader = default;
        static AccountView()
        {
            KeyHelper.PrepareKeys(typeof(AccountView));
        }
    }

    public static class UserPreferences
    {
        public static readonly string Title = default;
        public static readonly string PlayerProfileHeader = default;
        public static readonly string DisplayNameOption = default;
        public static readonly string UserInterfaceHeader = default;
        public static readonly string LanguageOption = default;
        public static readonly string UseManagedWindowDecorationsOption = default;
        public static readonly string AutoCloseServerOption = default;
        static UserPreferences()
        {
            KeyHelper.PrepareKeys(typeof(UserPreferences));
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