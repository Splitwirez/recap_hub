using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
#if DEBUG
            TabStripItem uiTestItem = new TabStripItem()
            {
                Content = "UI TESTS",
                DataContext = new UITestView()
            };
            (this.Find<TabStrip>("TopTabs").Items as AvaloniaList<object>).Add(uiTestItem);
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}