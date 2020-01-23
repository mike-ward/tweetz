using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Infrastructure.Converters
{
    public class ViewModelLocatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = parameter as Type ?? throw new NullReferenceException(nameof(parameter));
            return BootStrapper.GetService(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}