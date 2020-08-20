using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Views
{
    public partial class TweetProfileImageControl : UserControl
    {
        private const int MaxRetries = 3;
        private int Retries { get; set; }

        public TweetProfileImageControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty OriginalProperty = DependencyProperty.Register(
            nameof(Original),
            typeof(bool),
            typeof(TweetProfileImageControl));

        public bool Original
        {
            get => (bool)GetValue(OriginalProperty);
            set => SetValue(OriginalProperty, value);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            e.Handled = true;
            var imageControl = (Image)sender;

            if (Retries < MaxRetries)
            {
                Retries += 1;
                var user = (User)imageControl.DataContext;
                var uri = new Uri(user.ProfileImageUrl!);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.UriSource = uri;
                bitmap.EndInit();

                imageControl.Source = bitmap;
                TraceService.Message($"Retry ({Retries}) loading profile image: {uri}");
            }
            else
            {
                TraceService.Message(e.ErrorException.Message);
                var uri = new Uri("/Infrastructure/Resources/profile.png", UriKind.Relative);
                imageControl.Source = new BitmapImage(uri);
            }
        }
    }
}