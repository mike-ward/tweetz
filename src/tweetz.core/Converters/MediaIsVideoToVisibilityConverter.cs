using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Converters
{
    public class MediaIsVideoToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Media media)
            {
                var url = ImageViewerService.MediaSource(media);
                return ImageViewerService.IsMp4(url.ToString())
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}