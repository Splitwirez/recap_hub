using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        LocalPlayViewModel _localPlayVM = null;
        public LocalPlayViewModel LocalPlayVM
        {
            get => _localPlayVM;
            private set => RASIC(ref _localPlayVM, value);
        }

        public HomePageViewModel()
            : base()
        {
            LocalPlayVM = new LocalPlayViewModel()
            {
                GameConfigs = new ObservableCollection<GameConfigViewModel>()
                {
                    new GameConfigViewModel()
                    {
                        Title = "Darkspore",
                        Saves = new ObservableCollection<SaveGameViewModel>()
                        {
                            new SaveGameViewModel()
                            {
                                Title = "Campaign 1",
                                CurrentCrogenitorLevel = 23,
                                CurrentThreatLevel = "5-2",
                                Heroes = new ObservableCollection<HeroViewModel>()
                                {
                                    new HeroViewModel()
                                    {
                                        ShortName = "Blitz",
                                        LoreTitle = "The Storm Striker",
                                        Level = 1337,
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/blitz1.png") //Blitz, the Storm Striker_0x40626200!0x200e9795.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "Sage",
                                        LoreTitle = "The Life Forester",
                                        Level = 1337,
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/749013658_thumb.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "Goliath",
                                        LoreTitle = "Activation't???",
                                        Level = 1337,
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/canttouchdis.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "Zrin",
                                        LoreTitle = "The Sun Fist",
                                        Level = 1337,
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/zrin1.png") //Zrin, the Sun-Fist_0x40626200!0x200e979e.png")
                                    },
                                }
                            },
                            new SaveGameViewModel()
                            {
                                Title = "Campaign 2",
                                CurrentCrogenitorLevel = 10,
                                CurrentThreatLevel = "3-1",
                                Heroes = new ObservableCollection<HeroViewModel>()
                                {
                                    new HeroViewModel()
                                    {
                                        ShortName = "Blitz",
                                        LoreTitle = "The Storm Striker",
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/1667741389_thumb.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "Sage",
                                        LoreTitle = "The Life Forester",
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/sage1.png") //Sage, the Life Forester_0x40626200!0x200e97a1.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "Zrin",
                                        LoreTitle = "The Sun Fist",
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/576185321_thumb.png")
                                    },
                                    new HeroViewModel()
                                    {
                                        ShortName = "What",
                                        LoreTitle = "How",
                                        Thumbnail = GetBitmap("avares://ReCap.Hub/Res/Hero/0x6E76B39A.png")
                                    },
                                }
                            }
                        }
                    },
                    new GameConfigViewModel()
                    {
                        Title = "Campaign mods",
                    }
                }
            };

            if (LocalPlayVM.GameConfigs.Count > 0)
                LocalPlayVM.SelectedGameConfig = LocalPlayVM.GameConfigs[0];
            foreach (GameConfigViewModel gameConfig in LocalPlayVM.GameConfigs)
            {
                if (gameConfig.Saves.Count > 0)
                    gameConfig.SelectedSave = gameConfig.Saves[0];
            }
        }

        static Bitmap GetBitmap(string uri)
            => new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri(uri)));
    }
}
