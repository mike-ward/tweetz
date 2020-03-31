using System.Windows.Controls;
using tweetz.core.Services;

namespace tweetz.core.Controls
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
    }
}