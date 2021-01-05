using System.Globalization;
using System.Windows;
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

        private void OnDataContextChanged(object _, DependencyPropertyChangedEventArgs __)
        {
            SetVisibility();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name.IsEqualTo(nameof(Tag)))
            {
                SetVisibility();
            }

            base.OnPropertyChanged(e);
        }

        private void SetVisibility()
        {
            Visibility = Tag is bool val
                && !val
                && DataContext is TwitterStatus status
                && status.Language.IsNotEqualToIgnoreCase(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}