using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media.Imaging;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882 and Ivan Leonenko - http://stackoverflow.com/a/12638859/218882

namespace tweetz.core.Views.Controls
{
    /// <summary>
    ///     Represents a control that is a wrapper on System.Windows.Controls.Image for enabling filesystem-based caching
    /// </summary>
    public class Image : System.Windows.Controls.Image
    {
        static Image()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Image),
                new FrameworkPropertyMetadata(typeof(Image)));
        }

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(url)) return;

            var cachedImage = (Image)obj;
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.UriSource      = new Uri(url);
            bitmapImage.CreateOptions  = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.Default); // Enable IE-like cache policy.
            bitmapImage.EndInit();
            cachedImage.Source = bitmapImage;
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(
                "ImageUrl",
                typeof(string),
                typeof(Image),
                new PropertyMetadata(string.Empty, ImageUrlPropertyChanged));
    }
}