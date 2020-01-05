using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class ResizeModeConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && val
                ? ResizeMode.NoResize
                : ResizeMode.CanResize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}