using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Styling;

namespace ReCap.CommonUI
{
    public class BindToDynamicResourceExtension
    {
        readonly object _resKeyBinding;

        readonly Func<IServiceProvider, object> _resKeyProvideValue;
        public BindToDynamicResourceExtension(object resKeyBinding)
        {
            _resKeyBinding = resKeyBinding;
            Type type = _resKeyBinding.GetType();
            var methods = type.GetMethods();
            var m_ProvideValue = methods.FirstOrDefault(x => x.Name == nameof(ProvideValue));
            
            var paramCount = m_ProvideValue.GetParameters().Count();
            if (paramCount > 1)
                throw new Exception("how > 1");
            else if (paramCount < 0)
                throw new Exception("how < 0");
            else if (paramCount == 1)
                _resKeyProvideValue = p => m_ProvideValue.Invoke(_resKeyBinding, new object[] { p });
            else if (paramCount == 0)
                _resKeyProvideValue = p => m_ProvideValue.Invoke(_resKeyBinding, new object[0]);
            else
                throw new Exception("what\nhow");
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var resKey = _resKeyProvideValue(serviceProvider);
            return new DynamicResourceExtension(resKey);
        }
    }
/*
        : DynamicResourceExtension
    {
        
    }
*/
}