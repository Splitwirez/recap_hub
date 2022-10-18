using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    public class HubData : ViewModelBase
    {
        const string GAME_CONFIGS_EL = "gameConfigs";
        const string GAME_PATH_ATTR = "gameInstallPath";
        const string SAVES_PATH_ATTR = "savesPath";


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
                    if (gameConfigEl.TryGetAttributeValue(GAME_PATH_ATTR, out string gamePath)
                        && gameConfigEl.TryGetAttributeValue(SAVES_PATH_ATTR, out string savesPath))
                    {
                        GameConfigs.Add(
                            new GameConfigViewModel(gamePath)
                            //new GameConfigViewModel(gamePath, savesPath)
                            );
                    }
                }
            }

            GameConfigs.CollectionChanged += GameConfigs_CollectionChanged;

            if (immediatelyWrite)
                Save();
        }

        private void GameConfigs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var el = _doc.Root.Element(GAME_CONFIGS_EL);
            var els = el.Elements();
            if (e.NewItems != null)
            {
                foreach (GameConfigViewModel gameConfig in e.NewItems)
                {
                    if (!els.Any(x => XElementIsGameConfig(x, gameConfig)))
                    {
                        var gameConfigEl = new XElement("gameConfig");
                        gameConfigEl.SetAttributeValue(GAME_PATH_ATTR, gameConfig.GameInstallPath);
                        gameConfigEl.SetAttributeValue(SAVES_PATH_ATTR, gameConfig.SavesPath);

                        el.Add(gameConfigEl);
                    }
                }
            }
            
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
            if (!Directory.Exists(_cfgPath))
                Directory.CreateDirectory(_cfgPath);
            _doc.Save(_xmlPath);
        }
    }
}
