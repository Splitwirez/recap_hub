using ReCap.Hub.ViewModels;

namespace ReCap.UITest.ViewModels.Pages
{
    public class ScrollViewerViewModel
        : ViewModelBase
    {
        bool _autoHideScrollBars = false;
        public bool AutoHideScrollBars
        {
            get => _autoHideScrollBars;
            set => RASIC(ref _autoHideScrollBars, value);
        }
    }
}