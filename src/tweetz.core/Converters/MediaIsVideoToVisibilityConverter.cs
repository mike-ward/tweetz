using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Converters
{
    public class MediaIsVideoToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is Media media 
                && IsVideo(media)
                && values[1] is false
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private static bool IsVideo(Media media)
        {
            var url = ImageViewerService.MediaSource(media);
            return ImageViewerService.IsMp4(url.ToString());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Array.Empty<object>();
        }
    }
}