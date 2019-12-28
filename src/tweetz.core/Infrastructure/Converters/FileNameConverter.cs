using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class FileNameConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string val
                ? Path.GetFileName(val)
                : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}