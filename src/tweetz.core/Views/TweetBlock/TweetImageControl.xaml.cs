using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using tweetz.core.Infrastructure.Extensions;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Views
{
    public partial class TweetImageControl : UserControl
    {
        private const int MaxRetries = 3;
        private int Retries { get; set; }

        public TweetImageControl()
        {
            InitializeComponent();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs ea)
        {
            try
            {
                ea.Handled = true;
                var image = (Image)sender;

                if (Retries < MaxRetries)
                {
                    Retries++;
                    var uri = new Uri(((Media)image.DataContext).MediaUrl);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache | BitmapCreateOptions.IgnoreColorProfile;
                    bitmap.UriSource = uri;
                    bitmap.EndInit();

                    image.Source = bitmap;
                    TraceService.Message($"Retry ({Retries.ToStringInvariant()}) loading image: {uri.ToStringInvariant()}");
                }
                else
                {
                    var loadingIndicator = (TextBlock)image.Tag;
                    loadingIndicator.ToolTip = ea.ErrorException.Message;
                    loadingIndicator.Text = (string)Application.Current.FindResource("WarningSign");
                    TraceService.Message(ea.ErrorException.Message);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                TraceService.Message(ex.Message);
            }
        }
    }
}