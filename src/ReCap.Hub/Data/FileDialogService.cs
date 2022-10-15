using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    public static class FileDialogService
    {
        public static async Task<string[]> ShowOpenFileDialogAsync(ViewModelBase viewModel)
        {
            //var window = ResolveViewFromViewModel(parent);
            if (TryFindWindowByViewModel(viewModel, out Window window))
                return await new OpenFileDialog().ShowAsync(window);
            else
                return null;
        }

        private static IEnumerable<Window> Windows =>
            (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

        private static bool TryFindWindowByViewModel(ViewModelBase viewModel, out Window window)
        {
            window = Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext));
            return window != null;
        }
    }
}