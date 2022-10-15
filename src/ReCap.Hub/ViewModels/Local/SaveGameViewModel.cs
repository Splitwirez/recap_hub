using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Linq;

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
            set => RASIC(ref _title, value);
        }

        string _xmlPath = null;
        XDocument _doc = null;
        public void ReadFromXml(string xmlPath)
        {
            _xmlPath = xmlPath;
            string xmlText = File.ReadAllText(_xmlPath);
            _doc = XDocument.Parse(xmlText);

            Title = Path.GetFileNameWithoutExtension(_xmlPath); //TODO: Please tell me using filename as email when logging in is a bug, and not the server's intended behavior...
            //_doc.Root.Element(NAME_EL).Value;
        }

        void Create(string xmlPath)
            => Create(Title, xmlPath);
        public void Create(string title, string xmlPath)
        {
            Title = title;
            _xmlPath = xmlPath;
            XDocument doc = new XDocument();
            
            var rootEl = new XElement("user");
            rootEl.Add(new XElement(NAME_EL, title));
            rootEl.Add(new XElement(EMAIL_EL, title));
            rootEl.Add(new XElement(PASSWORD_EL, string.Empty));
            rootEl.Add(GetAccountElement());
            rootEl.Add(GetCreaturesElement());

            doc.Add(rootEl);

            _doc = doc;
            Save();
            ReadFromXml(xmlPath);
        }

        public void Save()
        {
            string dir = Path.GetDirectoryName(_xmlPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            _doc.Save(_xmlPath);
        }

        public SaveGameViewModel()
        {

        }


        public SaveGameViewModel(string title, string xmlPath)
        {
            Title = title;

            if (File.Exists(xmlPath))
            {
                ReadFromXml(xmlPath);
            }
            else
            {
                Create(title, xmlPath);
            }
        }

        XElement GetAccountElement()
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

        XElement GetCreaturesElement()
        {
            XElement el = new XElement(CREATURES_EL);
            foreach (HeroViewModel hero in Heroes)
            {
                el.Add(hero.ToXml());
            }
            return el;
        }
    }
}
