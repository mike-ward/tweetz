using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class ScreenNameConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string val
                ? $"@{val}"
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}