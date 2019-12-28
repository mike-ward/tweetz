using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class TimeAgoConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = value is DateTime ? (DateTime)value : default(DateTime);
            var timespan = DateTime.UtcNow - time;
            string Format(string s, double t) => string.Format(s, (int)t);
            if (timespan.TotalSeconds < 60) return Format("{0}s", timespan.TotalSeconds);
            if (timespan.TotalMinutes < 60) return Format("{0}m", timespan.TotalMinutes);
            if (timespan.TotalHours < 24) return Format("{0}h", timespan.TotalHours);
            if (timespan.TotalDays < 3) return Format("{0}d", timespan.TotalDays);
            return time.ToString("MMM d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}