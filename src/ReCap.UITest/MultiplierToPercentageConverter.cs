using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ReCap.UITest
{
    public class MultiplierToPercentageConverter
        : IValueConverter
    {
        const double _CONV_OP_BY = 100.0;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (double)value * _CONV_OP_BY;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (double)value / _CONV_OP_BY;
    }
}