using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class TextTrimConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return string.Empty;
            var text = value.ToString() ?? string.Empty;

            const int maxLength = 300;
            return text.Length > maxLength ? text.Substring(0, maxLength) + "…" : text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}