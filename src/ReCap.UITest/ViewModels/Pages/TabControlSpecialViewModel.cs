using System.Collections.ObjectModel;
using ReCap.Hub.ViewModels;

namespace ReCap.UITest.ViewModels.Pages
{
    public class TabControlSpecialViewModel
        : TabsViewModelBase
    {
        protected override ObservableCollection<PageTabViewModel> CreateTabs()
            => new()
            {
                new PageTabViewModel("Tab 0", null),
                new PageTabViewModel("Tab 1", null),
                new PageTabViewModel("Tab 2", null),
                new PageTabViewModel("Tab 3", null),
            };
    }
}