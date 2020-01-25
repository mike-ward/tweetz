using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class WindowStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && val
                ? WindowStyle.None
                : WindowStyle.SingleBorderWindow;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}