using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;


namespace ReCap.CommonUI
{
    public partial class ReCapTheme
        : Styles
        , IResourceNode
    {
        bool _useManagedChrome = false;
        public static readonly DirectProperty<ReCapTheme, bool> UseManagedChromeProperty = AvaloniaProperty.RegisterDirect<ReCapTheme, bool>(
            nameof(UseManagedChrome), o => o.UseManagedChrome, (o, v) => o.UseManagedChrome = v);
        public bool UseManagedChrome
        {
            get => _useManagedChrome;
            set => SetAndRaise(UseManagedChromeProperty, ref _useManagedChrome, value);
        }

        public const string MANAGED_CHROME_KEY = "ManagedChrome";
        readonly ResourceDictionary _managedChromeStyles;
#nullable enable
        public ReCapTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
#nullable restore

            return;
            _managedChromeStyles = (ResourceDictionary)Resources[MANAGED_CHROME_KEY];
            Resources.Remove(MANAGED_CHROME_KEY);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            return;
            if (change.Property == UseManagedChromeProperty)
            {
                bool newUseMgChrome = change.GetNewValue<bool>();
                
                if (newUseMgChrome && !Resources.MergedDictionaries.Contains(_managedChromeStyles))
                    Resources.MergedDictionaries.Add(_managedChromeStyles);
                else if ((!newUseMgChrome) && Resources.MergedDictionaries.Contains(_managedChromeStyles))
                    Resources.MergedDictionaries.Remove(_managedChromeStyles);
                
                Owner?.NotifyHostedResourcesChanged(ResourcesChangedEventArgs.Empty);
            }
        }
    }
}