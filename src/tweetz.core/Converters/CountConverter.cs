using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return value;
            var count = (int)value;
            const double k = 1000;

            if (count == 0) return " ";
            if (count < 999) return count.ToString(CultureInfo.InvariantCulture);
            if (count < 999999) return string.Format(CultureInfo.InvariantCulture, "{0:N1}K", count / k);
            return string.Format(CultureInfo.InvariantCulture, "{0:N1}M", count / (k * k));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}