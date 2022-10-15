using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public abstract partial class AccountViewModelBase : ViewModelBase
    {
        public const string TUTORIAL_COMPLETED_EL = "tutorial_completed";
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
    }
}
