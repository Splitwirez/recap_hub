using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    public static class HubGlobalPaths
    {
        const string GAME_CONFIGS_DIR_NAME = "GameConfigs";


        const string RECAP_DIR_NAME = "ResurrectionCapsule";
        const string RECAP_HUB_DIR_NAME = "Hub_POC_Demo";
        const string STORAGE_DIR_NAME = "GlobalStorage";
        const string CONFIG_DIR_NAME = "UserConfig";
        static string DefaultConfigBasePath
        {
            get => Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        , RECAP_DIR_NAME
                        , RECAP_HUB_DIR_NAME
                    );
        }
        static string DefaultStoragePath
        {
            get
            {
                if (OperatingSystem.IsWindows())
                {
                    return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                        , RECAP_DIR_NAME
                        , RECAP_HUB_DIR_NAME
                    );
                }
                else
                {
                    return Path.Combine(
                        DefaultConfigBasePath
                        , STORAGE_DIR_NAME
                    );
                }
            }
        }

        static string DefaultConfigPath
        {
            get
            {
                if (OperatingSystem.IsWindows())
                {
                    return Path.Combine(
                        DefaultConfigBasePath
                    );
                }
                else
                {
                    return Path.Combine(
                        DefaultConfigBasePath
                        , CONFIG_DIR_NAME
                    );
                }
            }
        }



        public static readonly Assembly ReCapHubAssembly = Assembly.GetExecutingAssembly();
        const string OVERRIDE_CFG_PATH = "overrideCfgPath.txt";
        public static string CfgPath
        {
            get => GetHubExternalDataPath(DefaultConfigPath);
        }

        static string GetHubExternalDataPath(string defaultPath)
        {
            string retPath = defaultPath;
            string overrideFilePath = Path.Combine(retPath, OVERRIDE_CFG_PATH);

            if (!Directory.Exists(retPath))
                Directory.CreateDirectory(retPath);
            else if (File.Exists(overrideFilePath))
            {
                string altPath = File.ReadAllLines(overrideFilePath)[0];
                try
                {
                    if (!Directory.Exists(altPath))
                        Directory.CreateDirectory(altPath);
                    retPath = altPath;
                }
                catch
                { }
            }

            return retPath;
        }

        public static string ServerDir
        {
            get => Path.Combine(CfgPath, "Server"); //_serverDirPath;
        }

        static string ServerStorageDir
        {
            get => Path.Combine(ServerDir, "storage");
        }
        public static string ServerAccountsDir
        {
            get => Path.Combine(ServerStorageDir, "users");
        }


        /*string _configPath = string.Empty;
        public string ConfigPath
        {
            get => _configPath;
            protected set => RASIC(ref _configPath, value);
        }

        string _storagePath = string.Empty;
        public string StoragePath
        {
            get => _storagePath;
            protected set => RASIC(ref _storagePath, value);
        }


        public string GameConfigsPath
        {
            get => Path.Combine(ConfigPath, GAME_CONFIGS_DIR_NAME);
        }*/
    }
}
