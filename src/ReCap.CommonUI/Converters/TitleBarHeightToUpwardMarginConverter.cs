using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI
{
    public class TitleBarHeightToUpwardMarginConverter
        : IValueConverter
    {
        public static readonly TitleBarHeightToUpwardMarginConverter Instance = new();
        private TitleBarHeightToUpwardMarginConverter()
        {}
        

        static double GetAsDouble(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is double val)
                return val;
                
            string objStr = (obj is string str)
                ? str
                : obj.ToString()
            ;
            return double.Parse(objStr);
        }
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = GetAsDouble(value);
            double showPortion = GetAsDouble(parameter);
            
            return new Thickness(0, -(height - showPortion), 0, 0);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
