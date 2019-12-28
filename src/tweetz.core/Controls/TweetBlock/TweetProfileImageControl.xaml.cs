using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Controls
{
    public partial class TweetProfileImageControl : UserControl
    {
        public TweetProfileImageControl()
        {
            InitializeComponent();
        }

        public bool Bigger
        {
            get { return (bool)GetValue(BiggerProperty); }
            set { SetValue(BiggerProperty, value); }
        }

        public static readonly DependencyProperty BiggerProperty = DependencyProperty.Register(
            "Bigger",
            typeof(bool),
            typeof(TweetProfileImageControl));
    }
}