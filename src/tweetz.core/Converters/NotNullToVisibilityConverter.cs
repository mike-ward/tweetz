using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null || (value is string val && string.IsNullOrWhiteSpace(val))
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}