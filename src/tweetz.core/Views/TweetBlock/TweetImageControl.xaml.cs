using System.Net;
using System.Windows;
using System.Windows.Controls;
using tweetz.core.Services;

namespace tweetz.core.Views
{
    public partial class TweetImageControl : UserControl
    {
        public TweetImageControl()
        {
            InitializeComponent();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs ea)
        {
            ea.Handled = true;
            var image = (Image)sender;
            var loadingIndicator = (TextBlock)image.Tag;
            loadingIndicator.Text = (string)Application.Current.FindResource("WarningSign");

            if (ea.ErrorException is WebException ex)
            {
                TraceService.Message($"{ex.Message} (status: {ex.Status})");
                ((Grid)image.Parent).Visibility = Visibility.Collapsed;
            }
            else
            {
                TraceService.Message(ea.ErrorException.Message);
            }
        }
    }
}