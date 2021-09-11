using System;
using System.Globalization;
using System.Windows.Data;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Converters
{
    internal class MediaToTextLinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Media media && IsVideo(media)
                ? "[video]"
                : "[image]";
        }

        private static bool IsVideo(Media media)
        {
            var url = ImageViewerService.MediaSource(media);
            return ImageViewerService.IsMp4(url.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}