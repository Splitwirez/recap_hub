using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ReCap.Hub.Data;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Models
{
    public class AccountModel : ModelBase
    {
        /*public const string TUTORIAL_COMPLETED_EL = "tutorial_completed";
        string _tutorialCompleted = "N";
        public string TutorialCompleted
        {
            get => _tutorialCompleted;
            set => RASIC(ref _tutorialCompleted, value);
        }

        public const string CHAIN_PROGRESSION_EL = "chain_progression";
        int _chainProgression = 0;
        public int ChainProgression
        {
            get => _chainProgression;
            set => RASIC(ref _chainProgression, value);
        }

        public const string HERO_ACTIVATIONS_EL = "creature_rewards";
        int _heroActivations = 1;
        public int HeroActivations
        {
            get => _heroActivations;
            set => RASIC(ref _heroActivations, value);
        }

        public const string CURRENT_GAME_ID_EL = "current_game_id";
        int _currentGameID = 1;
        public int CurrentGameID
        {
            get => _currentGameID;
            set => RASIC(ref _currentGameID, value);
        }

        public const string CURRENT_PLAYGROUP_ID_EL = "current_playgroup_id";
        int _currentPlaygroupID = 1;
        public int CurrentPlaygroupID
        {
            get => _currentPlaygroupID;
            set => RASIC(ref _currentPlaygroupID, value);
        }

        public const string DEFAULT_CAMPAIGN_SQUAD_ID_EL = "default_deck_pve_id";
        int _defaultCampaignSquadID = 1;
        public int DefaultCampaignSquadID
        {
            get => _defaultCampaignSquadID;
            set => RASIC(ref _defaultCampaignSquadID, value);
        }

        public const string DEFAULT_PVP_SQUAD_ID_EL = "default_deck_pvp_id";
        int _defaultPvPSquadID = 1;
        public int DefaultPvPSquadID
        {
            get => _defaultPvPSquadID;
            set => RASIC(ref _defaultPvPSquadID, value);
        }

        public const string AVATAR_ID_EL = "avatar_id";
        int _avatarID = 1;
        public int AvatarID
        {
            get => _avatarID;
            set => RASIC(ref _avatarID, value);
        }

        public const string BLAZE_ID_EL = "blaze_id";
        int _blazeID = 2383;
        public int BlazeID
        {
            get => _blazeID;
            set => RASIC(ref _blazeID, value);
        }

        public const string ID_EL = "id";
        int _id = 2383;
        public int ID
        {
            get => _id;
            set => RASIC(ref _id, value);
        }

        public const string DNA_EL = "dna";
        int _dna = 300;
        public int DNA
        {
            get => _dna;
            set => RASIC(ref _dna, value);
        }

        public const string NEW_PLAYER_INVENTORY_EL = "new_player_inventory";
        int _newPlayerInventory = 1;
        public int NewPlayerInventory
        {
            get => _newPlayerInventory;
            set => RASIC(ref _newPlayerInventory, value);
        }

        public const string NEW_PLAYER_PROGRESS_EL = "new_player_progress";
        int _newPlayerProgress = 9001;
        public int NewPlayerProgress
        {
            get => _newPlayerProgress;
            set => RASIC(ref _newPlayerProgress, value);
        }

        public const string CASHOUT_BONUS_TIME_EL = "cashout_bonus_time";
        int _cashoutBonusTime = 1;
        public int CashoutBonusTime
        {
            get => _cashoutBonusTime;
            set => RASIC(ref _cashoutBonusTime, value);
        }

        public const string STAR_LEVEL_EL = "star_level";
        int _starLevel = 1;
        public int StarLevel
        {
            get => _starLevel;
            set => RASIC(ref _starLevel, value);
        }

        public const string UNLOCK_CATALYSTS_EL = "unlock_catalysts";
        bool _unlockCatalysts = false;
        public bool UnlockCatalysts
        {
            get => _unlockCatalysts;
            set => RASIC(ref _unlockCatalysts, value);
        }

        public const string UNLOCK_DIAGONAL_CATALYSTS_EL = "unlock_diagonal_catalysts";
        bool _unlockDiagonalCatalysts = false;
        public bool UnlockDiagonalCatalysts
        {
            get => _unlockDiagonalCatalysts;
            set => RASIC(ref _unlockDiagonalCatalysts, value);
        }

        public const string UNLOCK_FUEL_TANKS_EL = "unlock_fuel_tanks";
        int _unlockFuelTanks = 1;
        public int UnlockFuelTanks
        {
            get => _unlockFuelTanks;
            set => RASIC(ref _unlockFuelTanks, value);
        }

        public const string INVENTORY_CAPACITY_EL = "unlock_inventory";
        int _inventoryCapacity = 250000;
        public int InventoryCapacity
        {
            get => _inventoryCapacity;
            set => RASIC(ref _inventoryCapacity, value);
        }

        public const string UNLOCK_PVE_DECKS_EL = "unlock_pve_decks";
        int _unlockPvEDecks = 2;
        public int UnlockPvEDecks
        {
            get => _unlockPvEDecks;
            set => RASIC(ref _unlockPvEDecks, value);
        }

        public const string UNLOCK_PVP_DECKS_EL = "unlock_pvp_decks";
        int _unlockPvPDecks = 1;
        public int UnlockPvPDecks
        {
            get => _unlockPvPDecks;
            set => RASIC(ref _unlockPvPDecks, value);
        }

        public const string UNLOCK_STATS_EL = "unlock_stats";
        int _unlockStats = 1;
        public int UnlockStats
        {
            get => _unlockStats;
            set => RASIC(ref _unlockStats, value);
        }

        public const string UNLOCK_INVENTORY_IDENTIFY_EL = "unlock_inventory_identify";
        int _unlockInventoryIdentify = 250000;
        public int UnlockInventoryIdentify
        {
            get => _unlockInventoryIdentify;
            set => RASIC(ref _unlockInventoryIdentify, value);
        }

        public const string EDITOR_DETAIL_SLOTS_EL = "unlock_editor_flair_slots";
        int _editorDetailSlots = 1;
        public int EditorDetailSlots
        {
            get => _editorDetailSlots;
            set => RASIC(ref _editorDetailSlots, value);
        }

        public const string UPSELL_EL = "upsell";
        int _upsell = 1;
        public int Upsell
        {
            get => _upsell;
            set => RASIC(ref _upsell, value);
        }

        public const string XP_EL = "xp";
        int _xp = 1;
        public int XP
        {
            get => _xp;
            set => RASIC(ref _xp, value);
        }

        public const string GRANT_ALL_ACCESS_EL = "grant_all_access";
        int _grantAllAccess = 1;
        public int GrantAllAccess
        {
            get => _grantAllAccess;
            set => RASIC(ref _grantAllAccess, value);
        }

        public const string GRANT_ONLINE_ACCESS_EL = "grant_online_access";
        int _grantOnlineAccess = 1;
        public int GrantOnlineAccess
        {
            get => _grantOnlineAccess;
            set => RASIC(ref _grantOnlineAccess, value);
        }

        public const string CAP_LEVEL_EL = "cap_level";
        int _capLevel = 0;
        public int CapLevel
        {
            get => _capLevel;
            set => RASIC(ref _capLevel, value);
        }

        public const string CAP_PROGRESSION_EL = "cap_progression";
        int _capProgression = 0;
        public int CapProgression
        {
            get => _capProgression;
            set => RASIC(ref _capProgression, value);
        }

        public const string GAME_SETTINGS_EL = "settings";
        string _gameSettings = "DisableLMBAttack,off;ShowAllyHealthAndManaBars,on;ShowAllyNames,on;ShowDamageDoneByAllies,off;ShowDamageDoneByMe,on;ShowDamageDoneToAllies,off;ShowDamageDoneToMe,on;ShowDamagedNPCHealthBars,on;ShowEnemyNames,on;ShowHealingDoneToAllies,off;ShowHealingDoneToEnemies,off;ShowHealingDoneToMe,on;ShowManaGainedOrLostByAllies,off;ShowManaGainedOrLostByMe,on;ShowMyHealthAndManaBars,on;ShowMyName,on;ShowPickupsByAllies,on;ShowPickupsByMe,on;ShowRadar,on;";
        public string GameSettings
        {
            get => _gameSettings;
            set => RASIC(ref _gameSettings, value);
        }

        public const string CROGENITOR_LEVEL_EL = "level";
        int _crogenitorLevel = 0;
        public int CrogenitorLevel
        {
            get => _crogenitorLevel;
            set => RASIC(ref _crogenitorLevel, value);
        }*/

        public static readonly XName ACCOUNT_DETAILS_EL = "account";
        public static readonly XName USER_EL = "user";
        public static readonly XName HEROES_EL = "creatures";
        public static readonly XName SQUADS_EL = "decks";
        public static readonly XName FEED_EL = "feed";
        public static readonly XName ITEMS_EL = "items";


        /*public const string EMAIL_EL = "email";
        string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => RASIC(ref _email, value);
        }

        
        public const string USER_NAME_EL = "name";
        string _userName = string.Empty;
        public string UserName
        {
            get => _userName;
            set => RASIC(ref _userName, value);
        }

        public const string PASSWORD = "password";
        string _password = string.Empty;
        public string Password
        {
            get => _password;
            protected set =>
                //TODO: muster the courage to even THINK about storing passwords
                RASIC(ref _password,
                    string.Empty //value
                );
        }*/
        const string NAME_EL = "name";
        const string EMAIL_EL = "email";
        const string PASSWORD_EL = "password";
        public readonly XmlStringProperty UserName = new(string.Empty, NAME_EL);
        public readonly XmlStringProperty EmailAddress = new(string.Empty, EMAIL_EL);
        public readonly XmlStringProperty Password = new(string.Empty, PASSWORD_EL);
        public readonly XmlStringProperty TutorialCompleted = new("N", ACCOUNT_DETAILS_EL, "tutorial_completed");
        public readonly XmlStringProperty ChainProgression = new("0", ACCOUNT_DETAILS_EL, "chain_progression");
        public readonly XmlProperty<int> HeroActivations = new(1, ACCOUNT_DETAILS_EL, "creature_rewards");
        public readonly XmlProperty<int> CurrentGameID = new(1, ACCOUNT_DETAILS_EL, "current_game_id");
        public readonly XmlProperty<int> CurrentPlaygroupID = new(1, ACCOUNT_DETAILS_EL, "current_playgroup_id");
        public readonly XmlProperty<int> DefaultCampaignSquadID = new(1, ACCOUNT_DETAILS_EL, "default_deck_pve_id");
        public readonly XmlProperty<int> DefaultPvPSquadID = new(1, ACCOUNT_DETAILS_EL, "default_deck_pvp_id");
        public readonly XmlProperty<int> CrogenitorLevel = new(3, ACCOUNT_DETAILS_EL, "level");
        public readonly XmlProperty<int> AvatarID = new(1, ACCOUNT_DETAILS_EL, "avatar_id");
        public readonly XmlProperty<int> BlazeID = new(2383, ACCOUNT_DETAILS_EL, "blaze_id");
        public readonly XmlProperty<int> ID = new(2383, ACCOUNT_DETAILS_EL, "id");
        public readonly XmlProperty<int> DNA = new(300, ACCOUNT_DETAILS_EL, "dna");
        public readonly XmlProperty<int> NewPlayerInventory = new(1, ACCOUNT_DETAILS_EL, "new_player_inventory");
        public readonly XmlProperty<int> NewPlayerProgress = new(9001, ACCOUNT_DETAILS_EL, "new_player_progress");
        public readonly XmlProperty<int> CashoutBonusTime = new(1, ACCOUNT_DETAILS_EL, "cashout_bonus_time");
        public readonly XmlProperty<int> StarLevel = new(1, ACCOUNT_DETAILS_EL, "star_level");
        public readonly XmlNumericalBoolProperty UnlockCatalysts = new(false, ACCOUNT_DETAILS_EL, "unlock_catalysts");
        public readonly XmlNumericalBoolProperty UnlockDiagonalCatalysts = new(false, ACCOUNT_DETAILS_EL, "unlock_diagonal_catalysts");
        public readonly XmlNumericalBoolProperty UnlockFuelTanks = new(false, ACCOUNT_DETAILS_EL, "unlock_fuel_tanks");
        public readonly XmlProperty<int> InventoryCapacity = new(0, ACCOUNT_DETAILS_EL, "unlock_inventory");
        public readonly XmlProperty<int> UnlockPvEDecks = new(0, ACCOUNT_DETAILS_EL, "unlock_pve_decks");
        public readonly XmlProperty<int> UnlockPvPDecks = new(0, ACCOUNT_DETAILS_EL, "unlock_pvp_decks");
        public readonly XmlProperty<int> UnlockStats = new(0, ACCOUNT_DETAILS_EL, "unlock_stats");
        public readonly XmlProperty<int> UnlockInventoryIdentify = new(0, ACCOUNT_DETAILS_EL, "unlock_inventory_identify");
        public readonly XmlProperty<int> UnlockEditorFlairSlots = new(0, ACCOUNT_DETAILS_EL, "unlock_editor_flair_slots");
        public readonly XmlNumericalBoolProperty Upsell = new(false, ACCOUNT_DETAILS_EL, "upsell");
        public readonly XmlProperty<int> Experience = new(0, ACCOUNT_DETAILS_EL, "xp");
        public readonly XmlProperty<int> GrantAllAccess = new(0, ACCOUNT_DETAILS_EL, "grant_all_access");
        public readonly XmlProperty<int> GrantOnlineAccess = new(0, ACCOUNT_DETAILS_EL, "grant_online_access");
        public readonly XmlProperty<int> CapLevel = new(0, ACCOUNT_DETAILS_EL, "cap_level");
        public readonly XmlProperty<int> CapProgression = new(0, ACCOUNT_DETAILS_EL, "cap_progression");
        public readonly XmlProperty<Dictionary<string, string>> Settings = new(
            (string inVal) =>
            {
                Dictionary<string, string> retVal = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(inVal) || string.IsNullOrWhiteSpace(inVal))
                    return retVal;
                
                string[] entries = inVal.Split(';', StringSplitOptions.RemoveEmptyEntries);
                
                
                foreach (string entry in entries)
                {
                    string[] subEntries = inVal.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (subEntries.Length == 2)
                    {
                        retVal[subEntries[0]] = subEntries[1];
                    }
                }

                return retVal;
            },
            (Dictionary<string, string> dict) =>
            {
                string retVal = string.Empty;
                foreach (string key in dict.Keys)
                {
                    retVal += $"{key},{dict[key]};";
                }
                return retVal;
            },
            ACCOUNT_DETAILS_EL, "settings");
        public readonly XmlElementsProperty<CreatureModel> Heroes = new(
            (XElement inVal) => CreatureModel.FromXml(inVal),
            (CreatureModel data) => data.ToXml(),
            /*insertElement:
            (seq, hero) =>
            {
                var replace = seq.FirstOrDefault(x => x.NounID == hero.NounID);
                if (replace != null)
                {
                    int index = seq.IndexOf(replace);
                    seq.Remove(replace);
                    seq.Insert(index, hero);
                }
                else
                    seq.Add(hero);
            },*/
            HEROES_EL);
        public readonly XmlElementsProperty<XElement> Squads = new(
            (XElement inVal) => inVal,
            (XElement data) => data,
            SQUADS_EL);

        /*string _displayName = string.Empty;
        public string DisplayName
        {
            get => _displayName;
            set => RASIC(ref _displayName, value);
        }*/


        /*protected static Func<XDocument, Action<Account>> FromXmlInternalAct =
            (XDocument doc) =>
            {
                return null;
            };
        
        static Action<Account> FromXmlInternal(XDocument doc)
        {
            return FromXmlInternalAct(doc);
        }*/



        public readonly string XmlPath = string.Empty;


        XDocument _doc = null;

        List<XmlPropertyBase> _xmlProperties = new List<XmlPropertyBase>();
        public AccountModel(string xmlPath)
        {
            XmlPath = xmlPath;

            _xmlProperties.Add(UserName);
            _xmlProperties.Add(EmailAddress);
            _xmlProperties.Add(Password);
            _xmlProperties.Add(TutorialCompleted);
            _xmlProperties.Add(ChainProgression);
            _xmlProperties.Add(HeroActivations);
            _xmlProperties.Add(CurrentGameID);
            _xmlProperties.Add(CurrentPlaygroupID);
            _xmlProperties.Add(DefaultCampaignSquadID);
            _xmlProperties.Add(DefaultPvPSquadID);
            _xmlProperties.Add(CrogenitorLevel);
            _xmlProperties.Add(AvatarID);
            _xmlProperties.Add(BlazeID);
            _xmlProperties.Add(ID);
            _xmlProperties.Add(DNA);
            _xmlProperties.Add(NewPlayerInventory);
            _xmlProperties.Add(NewPlayerProgress);
            _xmlProperties.Add(CashoutBonusTime);
            _xmlProperties.Add(StarLevel);
            _xmlProperties.Add(UnlockCatalysts);
            _xmlProperties.Add(UnlockDiagonalCatalysts);
            _xmlProperties.Add(UnlockFuelTanks);
            _xmlProperties.Add(InventoryCapacity);
            _xmlProperties.Add(UnlockPvEDecks);
            _xmlProperties.Add(UnlockPvPDecks);
            _xmlProperties.Add(UnlockStats);
            _xmlProperties.Add(UnlockInventoryIdentify);
            _xmlProperties.Add(UnlockEditorFlairSlots);
            _xmlProperties.Add(Upsell);
            _xmlProperties.Add(Experience);
            _xmlProperties.Add(GrantAllAccess);
            _xmlProperties.Add(GrantOnlineAccess);
            _xmlProperties.Add(CapLevel);
            _xmlProperties.Add(CapProgression);
            _xmlProperties.Add(Settings);
            _xmlProperties.Add(Heroes);
            //_xmlProperties.Add(Squads);

            if (!File.Exists(XmlPath))
            {
                string xmlDir = Path.GetDirectoryName(XmlPath);
                if (!Directory.Exists(xmlDir))
                    Directory.CreateDirectory(xmlDir);
                
                string title = Path.GetFileNameWithoutExtension(XmlPath);
                
                _doc = new XDocument();
                
                var feedEl = new XElement(FEED_EL);
                feedEl.Add(new XElement(ITEMS_EL));
                
                var userEl = new XElement(USER_EL);
                userEl.Add(new XElement(NAME_EL, title));
                userEl.Add(new XElement(EMAIL_EL, title));
                userEl.Add(new XElement(PASSWORD_EL));
                userEl.Add(new XElement(ACCOUNT_DETAILS_EL));
                userEl.Add(new XElement(HEROES_EL));
                //userEl.Add(new XElement(SQUADS_EL));
                //Heroes.WriteToXml(ref userEl);

                Squads.Sequence.Add(CreateSquad());
                Squads.WriteToXml(ref userEl);
                userEl.Add(feedEl);

                _doc.Add(userEl);
                SaveToXml();
            }
            RefreshFromXml(false);
        }

        XElement CreateSquad(params CreatureModel[] heroes)
        {
            var heroesEl = new XElement("creatures");
            foreach (var hero in heroes)
            {
                heroesEl.Add(hero.ToXml());
            };

            return new XElement("deck",
                new XElement("name", "Slot 1"),
                new XElement("id", 1),
                new XElement("slot", 1),
                new XElement("locked", 0),
                heroesEl
            );
        }


        public void RefreshFromXml(bool notify)
        {
            string xmlText = File.ReadAllText(XmlPath);
            _doc = XDocument.Parse(xmlText);
            
            RefreshFromXml(_doc, notify);
        }
        
        bool _firstRefresh = true;
        void RefreshFromXml(XDocument doc, bool notify)
        {
            var root = doc.Root;

            /*UserName = root.Element(USER_NAME_EL).Value;
            Email = root.Element(EMAIL_EL).Value;*/

            var rootEl = doc.Root;
            foreach (var prop in _xmlProperties)
            {
                prop.ReadFromXml(root, notify);
                if (_firstRefresh)
                    prop.PropertyChanged += WhenXmlPropertyChanged;
            }
            
            _firstRefresh = false;
#if !PASSWORD_STORAGE
            Password.Value = string.Empty;
#endif
        }

        public void SaveToXml()
        {
#if !PASSWORD_STORAGE
            Password.Value = string.Empty;
#endif

            var rootEl = _doc.Root;
            foreach (var prop in _xmlProperties)
            {
                prop.WriteToXml(ref rootEl);
            }

            //TODO: Fix stuff so writing doesn't have to be restricted like this...can't even tell if it's the hub or server that's broken tbh
            _doc.Save(XmlPath);
        }

        void WhenXmlPropertyChanged(object sender, EventArgs e)
        {
            SaveToXml();
        }

        public void DeleteDocument()
            => DeleteDocument(XmlPath);
        void DeleteDocument(string deleteThis)
        {
            if (File.Exists(deleteThis))
                File.Delete(deleteThis);
        }

        public void Rename(string newName)
        {
            string oldPath = XmlPath;
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName + Path.GetExtension(oldPath));
            //UserName.Value = newName;
            EmailAddress.Value = newName;
            File.Move(oldPath, newPath);
        }
    }

    /*public class AccountXmlTranslator : IModelXmlTranslator<AccountModelBase>
    {
        public Action<AccountModelBase> FromXml(XDocument doc)
        {
            return null;
        }
    }*/
}
