using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.MarkupExtensions;

using AvPropChangedArgs = Avalonia.AvaloniaPropertyChangedEventArgs;

namespace ReCap.CommonUI
{
    public class BindDynamicResource
        : AvaloniaObject
    {
        public static readonly AttachedProperty<object> ResourceKeyProperty =
            AvaloniaProperty.RegisterAttached<BindDynamicResource, Control, object>("ResourceKey", null);
        public static object GetResourceKey(Control control)
            => control.GetValue(ResourceKeyProperty);
        public static void SetResourceKey(Control control, double value)
            => control.SetValue(ResourceKeyProperty, value);


        static BindDynamicResource()
        {
            ResourceKeyProperty.Changed.AddClassHandler<ContentControl>(ContentControl_ResourceKeyProperty_Changed);
            ResourceKeyProperty.Changed.AddClassHandler<TemplatedControl>(TemplatedControl_ResourceKeyProperty_Changed);
            ResourceKeyProperty.Changed.AddClassHandler<TextBlock>(TextBlock_ResourceKeyProperty_Changed);
        }

        static void ContentControl_ResourceKeyProperty_Changed(ContentControl control, AvPropChangedArgs args)
            => ResourceKeyProperty_Changed(control, args, ContentControl.ContentProperty);
        static void TemplatedControl_ResourceKeyProperty_Changed(TemplatedControl control, AvPropChangedArgs args)
            => ResourceKeyProperty_Changed(control, args, TemplatedControl.TemplateProperty);
        static void TextBlock_ResourceKeyProperty_Changed(TextBlock control, AvPropChangedArgs args)
            => ResourceKeyProperty_Changed(control, args, TextBlock.TextProperty);
        /*
        {
            GetOldAndNewKey(args, out object oldKey, out object newKey);

            if (oldKey == newKey)
                return;
            //control.Bind(TextBlock.TextProperty, new DynamicResourceExtension(newKey))
        }
        */

        static readonly Dictionary<WeakReference<Control>, IDisposable> _bindings = new();
        static void ResourceKeyProperty_Changed<TControl>(TControl control, AvPropChangedArgs args, AvaloniaProperty prop)
            where TControl : Control
        {
            GetOldAndNewKey(args, out object oldKey, out object newKey);

            if (oldKey == newKey)
                return;
            
            //_bindings.Keys.FirstOrDefault(x => x.try)
            if (TryGetBindingFor(control, out WeakReference<Control> key, out IDisposable prevBinding))
            {
                _bindings.Remove(key);
                prevBinding.Dispose();
            }
            
            if (newKey == null)
                return;
            
            var newBinding = control.Bind(prop, new DynamicResourceExtension(newKey));
            _bindings.Add(new(control), newBinding);
        }

        static bool TryGetBindingFor(Control control, out WeakReference<Control> key, out IDisposable binding)
        {
            var keys = (_bindings != null)
                ? _bindings.Keys
                : null
            ;
            
            if (keys == null)
                goto fail;
            if (keys.Count <= 0)
                goto fail;


            for (int i = 0; i < _bindings.Keys.Count; i++)
            {
                var k = _bindings.Keys.ElementAt(i);
                if (k.TryGetTarget(out Control target))
                {
                    if (target != control)
                        continue;
                    
                    key = k;
                    binding = _bindings[k];
                    return true;
                }
                else
                {
                    _bindings.Remove(k);
                    i--;
                }
            }

            fail:
            key = default;
            binding = default;
            return false;
        }

        static void GetOldAndNewKey(AvPropChangedArgs args, out object oldKey, out object newKey)
            => (oldKey, newKey) = args.GetOldAndNewValue<object>();
    }
}
