﻿using System.Windows;
using System.Windows.Controls;
using tweetz.core.Commands;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Views.TweetBlock
{
    public partial class TweetRelatedLinkControl : UserControl
    {
        public TweetRelatedLinkControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            e.Handled = true;
            LongUrlService.HyperlinkToolTipOpeningHandler(sender, e);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // Hyperlinks in ItemsControls don't work unless the app first has focus.
            // Use a click handler to work around this.
            OpenLinkCommand.Command.Execute((DataContext as RelatedLinkInfo)?.Url, this);
        }
    }
}