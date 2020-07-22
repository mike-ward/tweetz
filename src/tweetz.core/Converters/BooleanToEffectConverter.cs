using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    internal class BooleanToEffectConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && val
            ? parameter
            : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}