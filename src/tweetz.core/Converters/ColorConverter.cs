using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Windows.Media.ColorConverter.ConvertFromString(value as string);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new System.Windows.Media.ColorConverter().ConvertToInvariantString(value)!;
        }
    }
}