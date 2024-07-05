using System;
using System.IO;
using System.Threading.Tasks;
using ReCap.Hub.Models;

namespace ReCap.Hub.ViewModels
{
    public class NewSaveGameViewModel
        : ViewModelBase
        , IDialogViewModel<SaveGameViewModel>
    {
        readonly TaskCompletionSource<SaveGameViewModel> _tcs = new TaskCompletionSource<SaveGameViewModel>();
        public TaskCompletionSource<SaveGameViewModel> CompletionSource
        {
            get => _tcs;
        }

        string _title = "New save game";
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        bool _isCloseable = true;
        public bool IsCloseable
        {
            get => _isCloseable;
            protected set => RASIC(ref _isCloseable, value);
        }

        string _savePath = string.Empty;
        public NewSaveGameViewModel(string savePath, bool isCloseable = true)
        {
            _savePath = savePath;
            IsCloseable = isCloseable;
        }

        public void Cancel(object _)
        {
            if (IsCloseable)
                _tcs.TrySetResult(null);
        }

        public void Accept(object _)
        {
            int id = 2;
            var blitzAlpha = CreateHero("1667741389", id, out CreatureModel blitzModel);
            id++;
            var sageAlpha = CreateHero("749013658", id, out CreatureModel sageModel);
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

            save.CreateSquad(); //blitzModel, sageModel);

            save.Save();
            _tcs.TrySetResult(save);
        }

        HeroViewModel CreateHero(string nounId, int id, out CreatureModel model)
        {
            CreatureModel mdl = new CreatureModel();
            mdl.NounID.Value = nounId;
            mdl.ID.Value = id;
            string url = $"{CreatureModel.PNG_URL_PRE}{nounId}{CreatureModel.PNG_URL_POST}";
            string urlLarge = $"{url}{CreatureModel.PNG_URL_POST_LARGE}";
            string urlSmall = urlLarge; //TODO: this is what the ReCap server does, but is that really...correct?
            mdl.PngLargeUrl.Value = urlLarge;
            mdl.PngThumbUrl.Value = urlSmall;
            model = mdl;
            return new HeroViewModel(mdl);
        }
    }
}
