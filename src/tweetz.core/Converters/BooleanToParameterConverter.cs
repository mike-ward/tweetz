using System;
using System.Globalization;
using System.Windows.Data;

namespace tweetz.core.Converters
{
    internal sealed class BooleanToParameterConverter : IValueConverter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "https://github.com/SonarSource/sonar-dotnet/issues/3123")]
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && val
            ? parameter
            : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}