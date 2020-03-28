using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace tweetz.core.Controls
{
    public partial class TweetProfileImageControl : UserControl
    {
        public TweetProfileImageControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty BiggerProperty = DependencyProperty.Register(
            "Bigger",
            typeof(bool),
            typeof(TweetProfileImageControl));

        public bool Bigger
        {
            get { return (bool)GetValue(BiggerProperty); }
            set { SetValue(BiggerProperty, value); }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var imageControl = (Image)sender;
            var uri = new Uri("/Infrastructure/Resources/profile.png", UriKind.Relative);
            imageControl.Source = new BitmapImage(uri);
        }
    }
}