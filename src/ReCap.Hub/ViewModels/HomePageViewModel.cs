using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ReCap.Hub.ViewModels
{
    public partial class HomePageViewModel : ViewModelBase
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
            LocalPlayVM = new LocalPlayViewModel();
        }

        static Bitmap GetBitmap(string uri)
            => new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>().Open(new Uri(uri)));
    }
}
