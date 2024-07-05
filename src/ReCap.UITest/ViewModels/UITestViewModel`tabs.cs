using System;
using System.Collections.ObjectModel;
using ReCap.UITest.ViewModels.Pages;

namespace ReCap.UITest.ViewModels
{
    public partial class UITestViewModel
        : TabsViewModelBase
    {
        protected override ObservableCollection<PageTabViewModel> CreateTabs()
            => new()
            {
                new PageTabViewModel("Button", new ButtonViewModel()),
                new PageTabViewModel("Closeable", new CloseableViewModel()),
                new PageTabViewModel("ComboBox", new ComboBoxViewModel()),
                new PageTabViewModel("ListBox", new ListBoxViewModel()),
                new PageTabViewModel("ScrollViewer", new ScrollViewerViewModel()),
                new PageTabViewModel("TabControl", new TabControlViewModel()),
                new PageTabViewModel("Specialized TabControl", new TabControlSpecialViewModel()),
                new PageTabViewModel("TextBox", new TextBoxViewModel()),
                new PageTabViewModel("WindowChrome", new WindowChromeViewModel()),
                new PageTabViewModel("AngledBorders", new AngledBordersViewModel()),
            };
    }
}