using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class NullToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string val)
            {
                return string.IsNullOrWhiteSpace(val)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return value == null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}