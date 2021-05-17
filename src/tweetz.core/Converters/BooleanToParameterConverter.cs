using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    internal sealed class BooleanToParameterConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true
                ? parameter
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}