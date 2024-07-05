using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ReCap.Hub.Models;

namespace ReCap.Hub.ViewModels
{
    public class SaveGameViewModel : AccountViewModelBase
    {
        const string NAME_EL = "name";
        const string EMAIL_EL = "email";
        const string PASSWORD_EL = "password";
        
        const string ACCOUNT_EL = "account";
        const string CREATURES_EL = "creatures";

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                RASIC(ref _title, value);
                if (!_initComplete)
                {
                    Model.UserName.Value = _title;
                    Model.EmailAddress.Value = _title;
                }
            }
        }

        public string EmailAddress
        {
            get => Model.EmailAddress.Value;
        }

        /*string _xmlPath = null;
        XDocument _doc = null;*/
        /*public void ReadFromXml(string xmlPath, double lastLaunchTime)
        {
            LastLaunchTime = lastLaunchTime;
            ReadFromXml(xmlPath);
        }*/
        public void ReadFromXml(string xmlPath, bool notify)
        {
            /*_xmlPath = xmlPath;*/
            ReadFromXml(notify);
        }
        public void ReadFromXml(bool notify)
        {
            EditModelHeroes = false;
            Model.RefreshFromXml(notify);
            EditModelHeroes = true;
            /*Heroes = new ObservableCollection<HeroViewModel>();
            string xmlText = File.ReadAllText(_xmlPath);
            _doc = XDocument.Parse(xmlText);

            Title = Path.GetFileNameWithoutExtension(_xmlPath); //TODO: Please tell me using filename as email when logging in is a bug, and not the server's intended behavior...
            //_doc.Root.Element(NAME_EL).Value;
            var creaturesEl = _doc.Root.Element(CREATURES_EL);
            /* //_autoAddToXml = false;
            var heroes = Heroes.ToList();
            foreach (var hero in heroes)
            {
                Heroes.Remove(hero);
            }
            //_autoAddToXml = true;* /
            foreach (var el in creaturesEl.Elements())
            {
                var hero = HeroViewModel.FromXml(el);
                Heroes.Add(hero);
            }
            
            //EnsureFSWatcher(Path.GetDirectoryName(_xmlPath));
            */
        }

        /*public bool EnableFSWatcher
        {
            get => _watcher?.EnableRaisingEvents ?? false;
            set
            {
                if (_watcher != null)
                    _watcher.EnableRaisingEvents = value;
            }
        }
        
        void EnsureFSWatcher(string directory)
        {
            if (_watcher == null)
            {
                _watcher = new FileSystemWatcher(directory);
                _watcher.Created += Watcher_FileOperationEventRaised;
                _watcher.Changed += Watcher_FileOperationEventRaised;
            }
            else
                _watcher.Path = directory;
        }

        void Watcher_FileOperationEventRaised(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Equals(_xmlPath, StringComparison.OrdinalIgnoreCase) && File.Exists(_xmlPath))
                ReadFromXml(_xmlPath);
        }*/

        /*void Create(string xmlPath)
            => Create(Title, xmlPath);*/
        public static SaveGameViewModel Create(string title, string xmlPath, int crogenitorLevel)
        {
            var saveGame = new SaveGameViewModel(xmlPath)
            {
                Title = title,
                CrogenitorLevel = crogenitorLevel
            };
            return saveGame;
            /*var saveGame = new SaveGameViewModel();
            saveGame._autoAddToXml = false;
            
            saveGame.Title = title;
            saveGame._xmlPath = xmlPath;
            XDocument doc = new XDocument();
            
            var rootEl = new XElement("user");
            rootEl.Add(new XElement(NAME_EL, title));
            rootEl.Add(new XElement(EMAIL_EL, title));
            rootEl.Add(new XElement(PASSWORD_EL, string.Empty));
            var accountEl = saveGame.GetNewAccountElement();
            accountEl.Element(CROGENITOR_LEVEL_EL).Value = crogenitorLevel.ToString();
            rootEl.Add(accountEl);
            foreach (HeroViewModel hero in heroes)
            {
                saveGame.Heroes.Add(hero);
            }
            saveGame._autoAddToXml = true;
            rootEl.Add(saveGame.GetNewCreaturesElement());

            doc.Add(rootEl);

            saveGame._doc = doc;
            saveGame.Save();
            saveGame.ReadFromXml(xmlPath);
            return saveGame;*/
        }

        public void WriteToXml(ref XElement el)
        {
            //Save();
            el.SetAttributeValue("id", Title);
            el.SetAttributeValue("lastLaunchTime", LastLaunchTime);
        }
        public void Save()
        {
            Model.SaveToXml();
            /*string dir = Path.GetDirectoryName(_xmlPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);*/
            
            /*var newHeroes = GetNewCreaturesElement();
            
            var oldHeroes = _doc.Element(CREATURES_EL);
            if (oldHeroes != null)
                oldHeroes.Remove();
            
            _doc.Root.Add(newHeroes);*/
            
            /*bool reEnableFSWatcher = EnableFSWatcher;
            EnableFSWatcher = reEnableFSWatcher;*/
            //////_doc.Save(_xmlPath);
            //EnableFSWatcher = reEnableFSWatcher;
        }

        //FileSystemWatcher _watcher = null;
        /*private SaveGameViewModel()
            : base()
        {

        }*/


        double _lastLaunchTime = -1;
        public double LastLaunchTime
        {
            get => _lastLaunchTime;
            set => _lastLaunchTime = value;
        }
        
        bool _initComplete = false;
        public SaveGameViewModel(string xmlPath)
            : base(new AccountModel(xmlPath)) //this()
        {
            Title = Path.GetFileNameWithoutExtension(xmlPath);
            /*Title = title;

            if (File.Exists(xmlPath))
            {*/
            ////ReadFromXml(xmlPath);
            /*}
            else
            {
                Create(title, xmlPath);
            }*/
            _initComplete = true;
        }

        public void CreateSquad(params CreatureModel[] heroes)
        {
            /*var heroesEl = new XElement("creatures");
            foreach (var hero in heroes)
            {
                heroesEl.Add(hero.ToXml());
            };*/

            Model.Squads.Sequence.Add(
                new DeckModel(1, heroes)
                /*new XElement("deck",
                    new XElement("name", "Slot 1"),
                    new XElement("id", 1),
                    new XElement("slot", 1),
                    new XElement("locked", 0),
                    heroesEl
                )*/
            );
        }

        /*XElement GetNewAccountElement()
        {
            XElement el = new XElement(ACCOUNT_EL);

            el.Add(
                new XElement(TUTORIAL_COMPLETED_EL, TutorialCompleted)
                , new XElement(CHAIN_PROGRESSION_EL, ChainProgression)
                , new XElement(HERO_ACTIVATIONS_EL, HeroActivations)
                , new XElement(CURRENT_GAME_ID_EL, CurrentGameID)
                , new XElement(DEFAULT_CAMPAIGN_SQUAD_ID_EL, DefaultCampaignSquadID)
                , new XElement(DEFAULT_PVP_SQUAD_ID_EL, DefaultPvPSquadID)
                , new XElement(CROGENITOR_LEVEL_EL, CrogenitorLevel)
                , new XElement(AVATAR_ID_EL, AvatarID)
                , new XElement(BLAZE_ID_EL, BlazeID)
                , new XElement(ID_EL, ID)
                , new XElement(DNA_EL, DNA)
                , new XElement(NEW_PLAYER_INVENTORY_EL, NewPlayerInventory)
                , new XElement(NEW_PLAYER_PROGRESS_EL, NewPlayerProgress)
                , new XElement(CASHOUT_BONUS_TIME_EL, CashoutBonusTime)
                , new XElement(STAR_LEVEL_EL, StarLevel)
                , new XElement(UNLOCK_CATALYSTS_EL, UnlockCatalysts)
                , new XElement(UNLOCK_DIAGONAL_CATALYSTS_EL, UnlockDiagonalCatalysts)
                , new XElement(UNLOCK_FUEL_TANKS_EL, UnlockFuelTanks)
                , new XElement(INVENTORY_CAPACITY_EL, InventoryCapacity)
                , new XElement(UNLOCK_PVE_DECKS_EL, UnlockPvEDecks)
                , new XElement(DEFAULT_PVP_SQUAD_ID_EL, UnlockPvPDecks)
                , new XElement(UNLOCK_STATS_EL, UnlockStats)
                , new XElement(UNLOCK_INVENTORY_IDENTIFY_EL, UnlockInventoryIdentify)
                , new XElement(EDITOR_DETAIL_SLOTS_EL, EditorDetailSlots)
                , new XElement(UPSELL_EL, Upsell)
                , new XElement(XP_EL, XP)
                , new XElement(GRANT_ALL_ACCESS_EL, GrantAllAccess)
                , new XElement(GRANT_ONLINE_ACCESS_EL, GrantOnlineAccess)
                , new XElement(CAP_LEVEL_EL, CapLevel)
                , new XElement(CAP_PROGRESSION_EL, CapProgression)
                , new XElement(GAME_SETTINGS_EL, GameSettings)
                );
            return el;
        }

        bool _autoAddToXml = true;
        XElement GetNewCreaturesElement()
        {
            XElement el = null;
            if ((_doc != null) && (_doc.Root != null))
                el = _doc.Root.Element(CREATURES_EL);
            
            if ((el != null) && (el.Elements().Count() > 0))
            {
                el.RemoveAll();
            }
            
            _autoAddToXml = false;
            el = new XElement(CREATURES_EL);
            foreach (HeroViewModel hero in Heroes)
            {
                el.Add(hero.ToXml());
            }
            _autoAddToXml = true;
            return el;
        }


        protected override void Heroes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_autoAddToXml)
                return;
            
            if (_doc == null)
                return;
            if (_doc.Root == null)
                return;
            
            XElement heroesEl = _doc.Root.Element(CREATURES_EL);
            if (heroesEl == null)
                return;
            

            if (e.NewItems != null)
            {
                foreach (HeroViewModel hero in e.NewItems)
                {
                    var nounID = hero.NounID;
                    RemoveHeroElement(heroesEl, nounID);
                    heroesEl.Add(hero.ToXml());
                }
            }

            if (e.OldItems != null)
            {
                foreach (HeroViewModel hero in e.OldItems)
                {
                    var nounID = hero.NounID;
                    RemoveHeroElement(heroesEl, nounID);
                }
            }
        }

        bool RemoveHeroElement(XElement heroesEl, string nounID)
        {
            foreach (var heroEl in heroesEl.Elements())
            {
                if (heroEl.Element(HeroViewModel.NOUN_ID_EL).Value == nounID)
                {
                    heroEl.Remove();
                    return true;
                    break;
                }
            }
            return false;
        }*/
    }
}
