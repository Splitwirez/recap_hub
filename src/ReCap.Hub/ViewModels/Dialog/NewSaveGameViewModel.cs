using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ReCap.Hub.Models;

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
            
            string xmlPath = Path.Combine(_savePath, fileTitle + ".xml");
            var save = SaveGameViewModel.Create(Title, xmlPath, 3);

            save.Heroes.Add(blitzAlpha);
            save.Heroes.Add(sageAlpha);
            
            save.Save();
            _tcs.TrySetResult(save);
        }

        HeroViewModel CreateHero(string nounId, int id)
        {
            CreatureModel mdl = new CreatureModel();
            mdl.NounID.Value = nounId;
            mdl.ID.Value = id;
            string url = $"{CreatureModel.PNG_URL_PRE}{nounId}{CreatureModel.PNG_URL_POST}";
            string urlLarge = $"{url}{CreatureModel.PNG_URL_POST_LARGE}";
            string urlSmall = urlLarge; //TODO: this is what the ReCap server does, but is that really...correct?
            mdl.PngLargeUrl.Value = urlLarge;
            mdl.PngThumbUrl.Value = urlSmall;
            
            return new HeroViewModel(mdl);
        }
    }
}
