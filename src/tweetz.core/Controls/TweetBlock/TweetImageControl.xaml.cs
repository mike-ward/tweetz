using System.Windows.Controls;

namespace tweetz.core.Controls
{
    public partial class TweetImageControl : UserControl
    {
        public TweetImageControl()
        {
            InitializeComponent();
            Loaded += TweetImageControl_Loaded;
        }

        private void TweetImageControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}