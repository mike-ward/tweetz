using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using tweetz.core.Extensions;
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
            try
            {
                e.Handled = true;
                var imageControl = (Image)sender;

                if (Retries < MaxRetries)
                {
                    Retries++;
                    var user = (User)imageControl.DataContext;
                    var uri = new Uri(user.ProfileImageUrlBigger!);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.IgnoreColorProfile;
                    bitmap.UriSource = uri;
                    bitmap.EndInit();

                    imageControl.Source = bitmap;
                    TraceService.Message($"Retry ({Retries.ToStringInvariant()}) loading profile image: {uri}");
                }
                else
                {
                    TraceService.Message(e.ErrorException.Message);
                    var uri = new Uri("/Resources/profile.png", UriKind.Relative);
                    imageControl.Source = new BitmapImage(uri);
                    Retries = 0;
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}