using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Reactive;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace ReCap.CommonUI
{
    public class StringFormatConverter
        : IMultiValueConverter
    {
        public static readonly StringFormatConverter Instance = new();
        private StringFormatConverter()
        {}


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
            string format = (string)values[0];
            
            object[] args = values
                .Skip(1)
                .ToArray()
            ;
            
            return string.Format(format, args);
        }

        
        object zConvert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            bool firstIsKey = false;
            //TryGetAsBoolean(parameter, out firstIsKey);
            
            if (parameter is IResourceNode node)
                firstIsKey = true;
            else
                node = null;

            string format = /*firstIsKey
                ? */
            (string)values[0];
            
            object[] args = values
                .Skip(1)
                .ToArray()
            ;
            
            return string.Format(format, args);
        }
    }
}
