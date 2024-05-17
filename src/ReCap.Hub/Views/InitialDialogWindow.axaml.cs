using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace ReCap.Hub.Views
{
    public partial class InitialDialogWindow : Window
    {
        public InitialDialogWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            var vm = new DialogOverlayVM();
            /*vm.PropertyChanged += (s, e) =>
            {
                if ((e.PropertyName == nameof(DialogOverlayVM.HasDialog)) && (!vm.HasDialog))
                {
                    DataContext = null;
                    Close();
                }
            };*/
            DataContext = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}