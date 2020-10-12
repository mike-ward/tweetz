using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) { return value; }
            var count = (int)value;
            const double k = 1000;

            return count switch
            {
                var c when c == 0 => " ",
                var c when c < 999 => count.ToString(CultureInfo.InvariantCulture),
                var c when c < 999999 => string.Format(CultureInfo.InvariantCulture, "{0:N1}K", count / k),
                _ => string.Format(CultureInfo.InvariantCulture, "{0:N1}M", count / (k * k))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}