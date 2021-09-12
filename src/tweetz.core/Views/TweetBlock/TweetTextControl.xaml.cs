using System.Windows.Controls;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Views.TweetBlock
{
    public partial class TweetTextControl : UserControl
    {
        public TweetTextControl()
        {
            InitializeComponent();
        }

        private void TextBlock_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs _)
        {
            if (sender is TextBlock textBlock && DataContext is TwitterStatus twitterStatus)
            {
                textBlock.Inlines.Clear();
                textBlock.Inlines.AddRange(FlowContentService.FlowContentInlines(twitterStatus, Settings.SettingsStatic!));
            }
        }
    }
}