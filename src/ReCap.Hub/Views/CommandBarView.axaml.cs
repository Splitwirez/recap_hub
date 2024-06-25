using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.Hub.Views
{
    public partial class CommandBarView
        : HeaderedItemsControl
    {
        public CommandBarView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}