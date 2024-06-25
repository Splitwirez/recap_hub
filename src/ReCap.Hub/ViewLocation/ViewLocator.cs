using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub.Views
{
    public class ViewLocator
        : IDataTemplate
        , ICachingDataTemplate
    {
        bool _useCaching = false;
        public bool UseCaching
        {
            get => _useCaching;
            set => _useCaching = value;
        }

        public virtual bool SupportsRecycling => false;

        Dictionary<object, Control> _cache = new();
        protected virtual Control BuildIfNeeded(object data)
        {
            Control ret = null;
            
            if ((data != null) && (data is RxObjectBase rxData))
            {
                var type = rxData.GetViewType();
            
                if ((type != null) && type.IsAssignableTo(typeof(Control)))
                {
                    var inst = Activator.CreateInstance(type);
                    ret = (Control)inst;
                }
            }
            
            return (ret != null)
                ? ret
                : new TextBlock()
                {
                    Text = $"Couldn't find view for type '{data.GetType().FullName}'"
                }
            ;
        }
        public virtual Control Build(object data)
            => this.Build_Impl(data, UseCaching, ref _cache, BuildIfNeeded);
        
        public virtual bool Match(object data)
            => data is ViewModelBase;
    }
}