using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class ProfileFollowUnFollowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true
                ? "Unfollow"
                : "Follow";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}