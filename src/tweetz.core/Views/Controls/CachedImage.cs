using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media.Imaging;

namespace tweetz.core.Views.Controls
{
    public class Image : System.Windows.Controls.Image
    {
        // Enable IE-like cache policy.
        private static RequestCachePolicy UriCachePolicy { get; } = new(RequestCacheLevel.Default);

        static Image()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Image),
                new FrameworkPropertyMetadata(typeof(Image)));
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(
                "ImageUrl",
                typeof(string),
                typeof(Image),
                new PropertyMetadata(string.Empty, ImageUrlPropertyChanged));

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
            bitmapImage.UriCachePolicy = UriCachePolicy; 
            bitmapImage.EndInit();
            cachedImage.Source = bitmapImage;
        }
    }
}