using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Views
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
    }
}