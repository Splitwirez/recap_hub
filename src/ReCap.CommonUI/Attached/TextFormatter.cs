using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Reactive;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ReCap.CommonUI
{
    public class TextFormatter
        : AvaloniaObject
    {
        public static readonly AttachedProperty<TextFormatter> FormatterProperty =
            AvaloniaProperty.RegisterAttached<TextFormatter, TextBlock, TextFormatter>("Formatter", null);
        public static TextFormatter GetFormatter(TextBlock control)
            => control.GetValue(FormatterProperty);
        public static void SetFormatter(TextBlock control, double value)
            => control.SetValue(FormatterProperty, value);



        /// <summary>
        /// Defines the <see cref="FormatKey"/> property.
        /// </summary>
        public static readonly StyledProperty<object> FormatKeyProperty =
            AvaloniaProperty.Register<TextFormatter, object>(nameof(FormatKey), null);

        public object FormatKey
        {
            get => GetValue(FormatKeyProperty);
            set => SetValue(FormatKeyProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="Format"/> property.
        /// </summary>
        public static readonly StyledProperty<string> FormatProperty =
            AvaloniaProperty.Register<TextFormatter, string>(nameof(Format), null);

        public string Format
        {
            get => GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }


        static TextFormatter()
        {
            FormatterProperty.Changed.AddClassHandler<TextBlock>(FormatterProperty_Changed);
            FormatKeyProperty.Changed.AddClassHandler<TextFormatter>(ResourceKeyProperty_Changed);
        }

        static void FormatterProperty_Changed(TextBlock arg1, AvaloniaPropertyChangedEventArgs arg2)
        {
            throw new NotImplementedException();
        }

        static void ResourceKeyProperty_Changed(TextFormatter s, AvaloniaPropertyChangedEventArgs args)
            => s?.OnResourceKeyPropertyChanged(args);

        IDisposable _prevBinding = null;
        void OnResourceKeyPropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            _prevBinding?.Dispose();


            var newKey = args.NewValue;
            if (newKey == null)
                return;

            var newBinding = this.Bind(FormatProperty, new DynamicResourceExtension(newKey));
            _prevBinding = newBinding;
        }


        static bool TryGetAsBoolean(object obj, out bool value)
        {
            if (obj == null)
            {
                value = default;
                return false;
            }

            if (obj is bool val)
            {
                value = val;
                return true;
            }

            string objStr = (obj is string str)
                ? str
                : obj.ToString()
            ;

            return bool.TryParse(objStr, out value);
        }


        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is TextBlock avObj))
                return BindingOperations.DoNothing;

            object[] args = values
                .Skip(1)
                .ToArray()
            ;

            return string.Format(Format, args);
        }
    }
}