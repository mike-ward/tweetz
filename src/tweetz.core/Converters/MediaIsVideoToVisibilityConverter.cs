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
            return value is Media media && IsVideo(media)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private static bool IsVideo(Media media)
        {
            var url = ImageViewerService.MediaSource(media);
            return ImageViewerService.IsMp4(url.ToString());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("General", "RCS1079:Throwing of new NotImplementedException.")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}