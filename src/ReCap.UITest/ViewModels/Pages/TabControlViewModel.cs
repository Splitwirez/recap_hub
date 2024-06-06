using System.Collections.ObjectModel;
using ReCap.Hub.ViewModels;

namespace ReCap.UITest.ViewModels.Pages
{
    public class TabControlViewModel
        : ViewModelBase
    {
        readonly ObservableCollection<PageTabViewModel> _tabs = new()
        {
            new PageTabViewModel("Tab 0", null),
            new PageTabViewModel("Tab 1", null),
            new PageTabViewModel("Tab 2", null),
            new PageTabViewModel("Tab 3", null),
        };
        public ObservableCollection<PageTabViewModel> Tabs
        {
            get => _tabs;
        }


        int _selectedIndex = 0;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => RASIC(ref _selectedIndex, value);
        }
    }
}