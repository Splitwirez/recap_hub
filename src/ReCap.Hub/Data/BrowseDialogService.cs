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
using Avalonia.Platform.Storage;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Data
{
    #nullable enable
    public static class BrowseDialogService
    {
        public static async Task<string?> OpenFileAsync(FilePickerOpenOptions options, Window? window = null)
        {
            options.AllowMultiple = false;
            var files = await InternalOpenFilesAsync(options, window);
            if ((files.Count > 0) && files.First().TryGetUri(out Uri? fileUri) && (fileUri != null))
            {
                return CleanUriPath(fileUri);
            }
            return null;
        }

        static readonly char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        const string DIR_PROTOCOL = "file://";
        static string CleanUriPath(string rawPath)
        {
            string cleanPath = Uri.UnescapeDataString(rawPath);
            
            if (cleanPath.StartsWith(DIR_PROTOCOL))
                cleanPath = cleanPath.Substring(DIR_PROTOCOL.Length);
            
            cleanPath = cleanPath.TrimStart('/');

            if (DIR_SEPARATOR != '/')
                cleanPath = cleanPath.Replace('/', DIR_SEPARATOR);
            if (DIR_SEPARATOR != '\\')
                cleanPath = cleanPath.Replace('\\', DIR_SEPARATOR);
            
            return cleanPath;
        }
        static string CleanUriPath(Uri uri)
            => CleanUriPath(uri.AbsoluteUri);


        public static async Task<IReadOnlyList<string>> OpenFilesAsync(FilePickerOpenOptions options, Window? window = null)
        {
            options.AllowMultiple = true;
            var files = await InternalOpenFilesAsync(options, window);
            List<string> filePaths = new List<string>();
            foreach (var file in files)
            {
                if (file.TryGetUri(out var fileUri) && (fileUri != null))
                {
                    filePaths.Add(CleanUriPath(fileUri));
                }
            }
            return filePaths.AsReadOnly();
        }

        #nullable disable
        static async Task<IReadOnlyList<IStorageFile>> InternalOpenFilesAsync(FilePickerOpenOptions options, Window window)
        {
            TopLevel win = window;
            if (win == null)
            {
                if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    win = desktop.MainWindow;
                }
            }
            return await win.StorageProvider.OpenFilePickerAsync(options);
        }
        /*public static async Task<string[]> ShowOpenFileDialogAsync(ViewModelBase viewModel)
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
        }*/
    }
}