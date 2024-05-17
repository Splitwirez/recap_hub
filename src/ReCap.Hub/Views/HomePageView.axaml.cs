using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public partial class HomePageView : UserControl
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
            var topTabs = this.FindControl<TabStrip>("TopTabs");
            topTabs.Items.Add(uiTestItem);
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}