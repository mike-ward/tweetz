using System.Windows.Controls;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Controls
{
    public partial class TweetTextControl : UserControl
    {
        public TweetTextControl()
        {
            InitializeComponent();
        }

        private void TextBlock_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (e.Property == TagProperty &&
                sender is TextBlock textBlock &&
                textBlock?.Tag is TwitterStatus twitterStatus)
            {
                e.Handled = true;
                textBlock.Inlines.Clear();
                textBlock.Inlines.AddRange(FlowContentService.FlowContentInlines(twitterStatus));
            }
        }
    }
}