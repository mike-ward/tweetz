using System.Windows;
using System.Windows.Controls;
using twitter.core.Models;

namespace tweetz.core.Controls
{
    public partial class TweetImageControl : UserControl
    {
        public TweetImageControl()
        {
            InitializeComponent();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            e.Handled = true;
            var image = (Image)sender;
            var loadingIndicator = (TextBlock)image.Tag;
            loadingIndicator.Text = (string)Application.Current.FindResource("WarningSign");
        }

        private void UnblockButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is TwitterStatus status)
            {
                status.IsSensitive = false;
            }
        }
    }
}