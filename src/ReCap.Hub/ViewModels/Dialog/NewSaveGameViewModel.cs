using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub.ViewModels
{
    public class NewSaveGameViewModel : ViewModelBase, IDialogViewModel<SaveGameViewModel>
    {
        TaskCompletionSource<SaveGameViewModel> _tcs = new TaskCompletionSource<SaveGameViewModel>();
        public TaskCompletionSource<SaveGameViewModel> GetCompletionSource()
        {
            return _tcs;
        }

        string _title = "New save game";
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        bool _allowsCancel = true;
        public bool AllowsCancel
        {
            get => _allowsCancel;
            protected set => RASIC(ref _allowsCancel, value);
        }

        string _savePath = string.Empty;
        public NewSaveGameViewModel(string savePath, bool allowsCancel = true)
        {
            _savePath = savePath;
            AllowsCancel = allowsCancel;
        }

        public void Cancel(object parameter)
        {
            if (AllowsCancel)
                _tcs.TrySetResult(null);
        }

        public void Accept(object parameter)
        {
            int id = 2;
            var blitzAlpha = CreateHero("1667741389", id);
            id++;
            var sageAlpha = CreateHero("749013658", id);
            id++;

            string fileTitle = Title;
            foreach (char c in Path.GetInvalidPathChars())
            {
                fileTitle = fileTitle.Replace(c, '-');
            }
            var save = new SaveGameViewModel(Title, Path.Combine(_savePath, fileTitle + ".xml"))
            {
                CrogenitorLevel = 1,
                Heroes =
                {
                    blitzAlpha,
                    sageAlpha
                }
            };
            _tcs.TrySetResult(save);
        }

        HeroViewModel CreateHero(string nounId, int id)
        {

            string url = $"{HeroViewModel.PNG_URL_PRE}{nounId}{HeroViewModel.PNG_URL_POST}";
            string urlLarge = $"{url}{HeroViewModel.PNG_URL_POST_LARGE}";
            string urlSmall = urlLarge; //TODO: this is what the ReCap server does, but is that really...correct?
            var hero = new HeroViewModel()
            {
                PngLargeUrl = urlLarge,
                PngSmallUrl = urlSmall,
                NounID = nounId,
                ID = id
            };
            return hero;
        }
    }
}
