using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace ReCap.Hub.Views
{
    public interface ICachingDataTemplate
        : IDataTemplate
    {
        public bool UseCaching { get; }
    }
    public static class ICachingDataTemplate_DefaultImpl
    {
        public static Control Build_Impl(this ICachingDataTemplate self, object data, bool useCaching, ref Dictionary<object, Control> _cache, Func<object, Control> createIfNeeded) //, ref Dictionary<object, Control> _cache)
        {
            if (useCaching && _cache.TryGetValue(data, out Control cached))
                return cached;

            Control ret = createIfNeeded(data);

            if (useCaching && !_cache.ContainsKey(data))
                _cache.Add(data, ret);
                
            return ret;
        }
    }
}