using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    internal class FontSmallerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value is double val
                ? Math.Max(10, val - 0.5)
                : value;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}