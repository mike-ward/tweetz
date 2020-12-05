using System.Globalization;
using System.Windows.Controls;
using tweetz.core.Extensions;
using twitter.core.Models;

namespace tweetz.core.Views
{
    public partial class TweetTranslateControl : UserControl
    {
        public TweetTranslateControl()
        {
            InitializeComponent();
        }

        private void OnDataContextChanged(object _, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Visibility = e.NewValue is TwitterStatus status && status.Language.IsNotEqualToIgnoreCase(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;
        }
    }
}