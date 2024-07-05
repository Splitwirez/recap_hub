using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI
{
    public class AddAllConverter
        : IMultiValueConverter
    {
        public static readonly AddAllConverter Instance = new AddAllConverter();
        private AddAllConverter()
        {}

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            double ret = 0;
            foreach (var value in values)
            {
                ret += NumberConvUtils.ObjectToDouble(value);
            }
            return ret;
        }
    }
}
