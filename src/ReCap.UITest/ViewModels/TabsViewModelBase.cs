using System.Collections.ObjectModel;
using ReCap.Hub.ViewModels;

namespace ReCap.UITest.ViewModels
{
    public abstract class TabsViewModelBase
        : ViewModelBase
    {
        protected abstract ObservableCollection<PageTabViewModel> CreateTabs();
        readonly ObservableCollection<PageTabViewModel> _tabs;
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

        public bool IsTabIndexvalid(int index)
        {
            int tabCount = Tabs.Count;
            if (tabCount <= 0)
                return false;
            
            if (index >= tabCount)
                return false;
            else if (index < 0)
                return false;
            
            return true;
        }

        public int WrapTabIndex(int rawIndex)
        {
            int newIndex = rawIndex;
            
            int tabCount = Tabs.Count;
            while (newIndex >= tabCount)
            {
                newIndex -= tabCount;
            }
            
            while (newIndex < 0)
            {
                newIndex += tabCount;
            }
            
            return newIndex;
        }

        public TabsViewModelBase()
            : base()
        {
            _tabs = CreateTabs();
        }
    }
}