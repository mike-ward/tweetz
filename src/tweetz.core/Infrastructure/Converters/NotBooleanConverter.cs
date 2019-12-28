using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class NotBooleanConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}