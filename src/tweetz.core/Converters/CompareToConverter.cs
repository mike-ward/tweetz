using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class CompareToConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.CompareOrdinal(value as string, parameter as string) == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}