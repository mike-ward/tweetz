using System.Windows;
using System.Windows.Controls;
using twitter.core.Models;

namespace tweetz.core.Views
{
    public partial class TweetBlockControl : UserControl
    {
        public TweetBlockControl()
        {
            InitializeComponent();
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