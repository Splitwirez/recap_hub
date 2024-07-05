using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    public class HubData : ViewModelBase
    {
        public const string GAME_CONFIGS_EL = "gameConfigs";
        public const string GAME_CONFIG_EL = "gameConfig";
        public const string GAME_PATH_ATTR = "gameInstallPath";
        public const string SAVES_PATH_ATTR = "savesPath";
        public const string WINE_PFX_ATTR = "winePrefix";
        public const string WINE_EX_ATTR = "wineExecutable";

        public const string USER_PREFS_EL = "preferences";


        /*static string DefaultCfgPath
        {
            get => Path.Combine(
                  OperatingSystem.IsWindows()
                    ? Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                    : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                , "ResurrectionCapsule"
                , "Hub_POC_Demo"
            );
        }

        const string OVERRIDE_CFG_PATH = "overrideCfgPath.txt";*/
        static string GetCfgPath()
        {
            /*string cfgPath = DefaultCfgPath;
            string cfgPathOverride = Path.Combine(cfgPath, OVERRIDE_CFG_PATH);
            
            if (!Directory.Exists(cfgPath))
                Directory.CreateDirectory(cfgPath);
            else if (File.Exists(cfgPathOverride))
            {
                string altCfgPath = File.ReadAllLines(cfgPathOverride)[0];
                if (!Directory.Exists(altCfgPath))
                    Directory.CreateDirectory(altCfgPath);
                cfgPath = altCfgPath;
            }
            
            return cfgPath;*/
            return HubGlobalPaths.CfgPath;
        }

        public static readonly HubData Instance = new HubData(GetCfgPath());

        ObservableCollection<GameConfigViewModel> _gameConfigs = new ObservableCollection<GameConfigViewModel>();
        public ObservableCollection<GameConfigViewModel> GameConfigs
        {
            get => _gameConfigs;
            protected set => RASIC(ref _gameConfigs, value);
        }

        string _cfgPath = string.Empty;
        public string CfgPath
        {
            get => _cfgPath;
        }
        string _xmlPath = string.Empty;
        XDocument _doc = null;
        private HubData(string path)
        {
            _cfgPath = path;
            _xmlPath = Path.Combine(_cfgPath, "config.xml");
            ReadXml();
        }

        const string _USER_DISPLAY_NAME_EL = "userDisplayName";
        string _userDisplayName = "Splitwirez"; //TODO: Don't hardcode yourself, idiot
        public string UserDisplayName
        {
            get => _userDisplayName;
            set => RASIC(ref _userDisplayName, value);
        }

        const string _USE_MANAGED_DECORATIONS_EL = "useManagedWindowDecorations";
        bool _useManagedDecorations = CommonUI.OSInfo.IsWindows;
        bool _useManagedDecorationsSet = false;
        public bool UseManagedDecorations
        {
            get => _useManagedDecorations;
            set
            {
                RASIC(ref _useManagedDecorations, value);
                _useManagedDecorationsSet = true;
            }
        }

        const string _AUTO_CLOSE_SERVER_EL = "autoCloseServer";
        bool _autoCloseServer = true;
        bool _autoCloseServerSet = false;
        public bool AutoCloseServer
        {
            get => _autoCloseServer;
            set
            {
                RASIC(ref _autoCloseServer, value);
                _autoCloseServerSet = true;
            }
        }


        void ReadXml()
        {
            bool immediatelyWrite = false;
            if (File.Exists(_xmlPath))
            {
                string xmlText = File.ReadAllText(_xmlPath);
                _doc = XDocument.Parse(xmlText);
            }
            else
            {
                _doc = XDocument.Parse("<hub></hub>");
                immediatelyWrite = true;
            }
            XElement gameConfigsEl = _doc.Root.Element(GAME_CONFIGS_EL);

            if (gameConfigsEl == null)
            {
                _doc.Root.Add(new XElement(GAME_CONFIGS_EL));
            }
            else
            {
                foreach (XElement gameConfigEl in gameConfigsEl.Elements())
                {
                    XElement el = gameConfigEl;
                    if (gameConfigEl.TryGetAttributeValue(GAME_PATH_ATTR, out string gamePath)
                        && gameConfigEl.TryGetAttributeValue(SAVES_PATH_ATTR, out string savesPath))
                    {
                        var gameConfigVM = new GameConfigViewModel(gamePath,
                            //new GameConfigViewModel(gamePath, savesPath)
                            gameConfigEl.TryGetAttributeValue(WINE_EX_ATTR, out string wineExec)
                                ? wineExec
                                : null
                            ,
                            gameConfigEl.TryGetAttributeValue(WINE_PFX_ATTR, out string winePrefix)
                                ? winePrefix
                                : null
                            , ref el);
                        
                        GameConfigs.Add(gameConfigVM);
                    }
                }
            }

            XElement userPrefsEl = _doc.Root.Element(USER_PREFS_EL);
            if (userPrefsEl != null)
            {
                if (userPrefsEl.TryGetElementValue(_USER_DISPLAY_NAME_EL, out string userDisplayName))
                    UserDisplayName = userDisplayName;

                if (userPrefsEl.TryGetElementBool(_USE_MANAGED_DECORATIONS_EL, out bool useManagedDecorations))
                    UseManagedDecorations = useManagedDecorations;

                if (userPrefsEl.TryGetElementBool(_AUTO_CLOSE_SERVER_EL, out bool autoCloseServer))
                    AutoCloseServer = autoCloseServer;
            }

            GameConfigs.CollectionChanged += GameConfigs_CollectionChanged;

            if (immediatelyWrite)
                Save();
        }

        private void GameConfigs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var el = _doc.Root.Element(GAME_CONFIGS_EL);
            var els = el.Elements();
            
            /*if ((e.NewItems != null) || (e.Action == NotifyCollectionChangedAction.Reset))
            {
                el.RemoveNodes();

                foreach (GameConfigViewModel gameConfig in e.NewItems)
                {
                    /*var gameConfigEl = els.FirstOrDefault(x => XElementIsGameConfig(x, gameConfig));

                    if (gameConfigEl != null)
                    {
                        /*gameConfigEl.SetAttributeValue(WINE_PFX_ATTR, gameConfig.WinePrefixPath);
                        gameConfigEl.SetAttributeValue(WINE_EX_ATTR, gameConfig.WineExecPath);
                        gameConfigEl.SetAttributeValue(GAME_PATH_ATTR, gameConfig.GameInstallPath);
                        gameConfigEl.SetAttributeValue(SAVES_PATH_ATTR, gameConfig.SavesPath);* /
                        gameConfig.WriteToXml(ref gameConfigEl);
                        gameConfigEl.Remove();
                        el.Add(gameConfigEl);
                    }
                    else
                    {* /
                        XElement newElement = new XElement(GAME_CONFIG_EL);
                        gameConfig.WriteToXml(ref newElement);
                        el.Add(newElement);
                    //}
                }
            }
            else
            {
                els = el.Elements();
                if (e.OldItems != null)
                {
                    foreach (GameConfigViewModel gameConfig in e.OldItems)
                    {
                        foreach (XElement gameConfigEl in els)
                        {
                            if (XElementIsGameConfig(gameConfigEl, gameConfig))
                            {
                                gameConfigEl.Remove();
                                break;
                            }
                        }
                    }
                }
            }*/

            Save();
        }

        bool XElementIsGameConfig(XElement x, GameConfigViewModel gameConfig)
            => XElementIsGameConfig(x, gameConfig.GameInstallPath, gameConfig.SavesPath);
        bool XElementIsGameConfig(XElement x, string gamePath, string savesPath)
        {
            if (x.TryGetAttributeValue(GAME_PATH_ATTR, out string xGamePath)
                        && x.TryGetAttributeValue(SAVES_PATH_ATTR, out string xSavesPath))
            {
                return
                       xGamePath.Equals(gamePath, StringComparison.OrdinalIgnoreCase)
                    && xSavesPath.Equals(savesPath, StringComparison.OrdinalIgnoreCase)
                ;
            }
            return false;
        }

        public void Save()
        {
            var el = _doc.Root.Element(GAME_CONFIGS_EL);
            el.RemoveNodes();
            foreach (var gameConfig in GameConfigs.OrderBy(x => x.LastLaunchTime))
            {
                var childEl = new XElement(GAME_CONFIG_EL);
                gameConfig.WriteToXml(ref childEl);
                //e.NewItems.Cast<GameConfigViewModel>().OrderBy(x => x.LastLaunchTime)
                el.Add(childEl);
            }

            var userPrefsEl = _doc.Root.Element(USER_PREFS_EL);
            bool mustCreateUserPrefsEl = userPrefsEl == null;
            if (mustCreateUserPrefsEl)
                userPrefsEl = new XElement(USER_PREFS_EL);
                
            if (_useManagedDecorationsSet)
                userPrefsEl.Add(new XElement(_USE_MANAGED_DECORATIONS_EL, UseManagedDecorations));

            if (_autoCloseServerSet)
                userPrefsEl.Add(new XElement(_AUTO_CLOSE_SERVER_EL, AutoCloseServer));

            if (mustCreateUserPrefsEl)
                _doc.Root.Add(userPrefsEl);

            if (!Directory.Exists(_cfgPath))
                Directory.CreateDirectory(_cfgPath);
            _doc.Save(_xmlPath);
        }
    }
}
