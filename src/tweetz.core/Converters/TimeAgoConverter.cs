using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = value is DateTime val
                ? val
                : default;

            var timespan = DateTime.UtcNow - time;
            static string Format(string s, double t) => string.Format(CultureInfo.InvariantCulture, s, (int)t);

            return timespan switch {
                { TotalSeconds: < 60 } => Format("{0}s", timespan.TotalSeconds),
                { TotalMinutes: < 60 } => Format("{0}m", timespan.TotalMinutes),
                { TotalHours  : < 24 } => Format("{0}h", timespan.TotalHours),
                { TotalDays   : < 10 } => Format("{0}d", timespan.TotalDays),
                _                      => time.ToString("MMM d", CultureInfo.CurrentUICulture)
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("General", "RCS1079:Throwing of new NotImplementedException.")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}