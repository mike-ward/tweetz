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
            loadingIndicator.ToolTip = ea.ErrorException.Message;
            loadingIndicator.Text = (string)Application.Current.FindResource("WarningSign");
            TraceService.Message(ea.ErrorException.Message);
        }
    }
}