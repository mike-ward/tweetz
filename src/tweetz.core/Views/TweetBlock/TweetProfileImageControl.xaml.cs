using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using tweetz.core.Services;

namespace tweetz.core.Views
{
    public partial class TweetProfileImageControl : UserControl
    {
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
            TraceService.Message(e.ErrorException.Message);
            var imageControl = (Image)sender;
            var uri = new Uri("/Infrastructure/Resources/profile.png", UriKind.Relative);
            imageControl.Source = new BitmapImage(uri);
        }
    }
}