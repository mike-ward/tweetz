using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class TextTrimConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string ?? string.Empty;
            const int maxLength = 300;

            return text.Length > maxLength
                ? text.Substring(0, maxLength) + "…"
                : text;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("General", "RCS1079:Throwing of new NotImplementedException.")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}