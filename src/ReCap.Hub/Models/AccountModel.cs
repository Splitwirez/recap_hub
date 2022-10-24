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
        public readonly XmlICanIntoXmlElementsProperty<CreatureModel> Heroes = new(HEROES_EL);
        /*public readonly XmlElementsProperty<CreatureModel> Heroes = new(
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
            },* /
            HEROES_EL);*/
        public readonly XmlICanIntoXmlElementsProperty<DeckModel> Squads = new(SQUADS_EL);

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



        public string _xmlPath = null;
        public string XmlPath
        {
            get => _xmlPath;
        }

        public bool TrySpecifyXmlPath(string newPath)
        {
            if (_xmlPath == null)
            {
                _xmlPath = newPath;
                EnsureReadFromXml();
                return true;
            }
            
            return false;
        }


        XDocument _doc = null;

        readonly List<XmlPropertyBase> _xmlProperties = new List<XmlPropertyBase>();
        public AccountModel()
        {
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
            _xmlProperties.Add(Squads);
            //Squads.Sequence.CollectionChanged += Squads_CollectionChanged;
        }

        private void Squads_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (DeckModel deck in e.OldItems)
                {

                }
            }

            List<DeckModel> newDecks = new List<DeckModel>();
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {

            }
            else if (e.NewItems != null)
            {
                //newDecks = new(e.NewItems);
            }
        }

        public AccountModel(string xmlPath)
            : this()
        {
            _xmlPath = xmlPath;
            EnsureReadFromXml();
        }

        void EnsureReadFromXml()
        {
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
                userEl.Add(new XElement(SQUADS_EL));
                //Heroes.WriteToXml(ref userEl);

                //Squads.Sequence.Add(CreateSquad());
                //Squads.WriteToXml(ref userEl);
                userEl.Add(feedEl);

                _doc.Add(userEl);
                SaveToXml();
            }
            RefreshFromXml(false);
        }

        /*XElement CreateSquad(params CreatureModel[] heroes)
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
        }*/


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

            foreach (var prop in _xmlProperties)
            {
                prop.ReadFromXml(root, notify);
                if (_firstRefresh)
                    prop.PropertyChanged += WhenXmlPropertyChanged;
            }

            if (_firstRefresh)
            {
                EnsureSquadHeroes(false);
            }
            
            _firstRefresh = false;
#if !PASSWORD_STORAGE
            Password.Value = string.Empty;
#endif
        }

        void EnsureSquadHeroes(bool onlyIfNull)
        {
            foreach (var deck in Squads.Sequence)
            {
                deck.EnsureHeroes(Heroes.Sequence, onlyIfNull);
            }
        }

        public void SaveToXml()
        {
#if !PASSWORD_STORAGE
            Password.Value = string.Empty;
#endif

            EnsureSquadHeroes(false);
            var root = _doc.Root;
            foreach (var prop in _xmlProperties)
            {
                prop.WriteToXml(ref root);
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
