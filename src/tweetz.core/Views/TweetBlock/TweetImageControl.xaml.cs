using System.Diagnostics;
using System.Net;
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

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs ea)
        {
            ea.Handled = true;
            var image = (Image)sender;
            var loadingIndicator = (TextBlock)image.Tag;
            loadingIndicator.Text = (string)Application.Current.FindResource("WarningSign");
            Trace.TraceError(ea.ErrorException.Message);

            if (ea.ErrorException is WebException ex &&
                ex.Status == WebExceptionStatus.ProtocolError)
            {
                ((Grid)image.Parent).Visibility = Visibility.Collapsed;
            }
        }
    }
}