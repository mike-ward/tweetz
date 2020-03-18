using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string val)
            {
                return string.IsNullOrWhiteSpace(val)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            return value is null
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}