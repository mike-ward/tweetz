using System.Windows.Controls;
using System.Windows.Media;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class TimelineControl : UserControl
    {
        public TimelineControl()
        {
            InitializeComponent();
        }

        public void ScrollToHome()
        {
            // is there an easier way to find the ScrollViewer?
            if (VisualTreeHelper.GetChildrenCount(ItemsControl) > 0)
            {
                var scrollViewer = VisualTreeHelper.GetChild(ItemsControl, 0) as ScrollViewer;
                scrollViewer?.ScrollToHome();
            }
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Only want the home timeline to pause on scroll
            // Why, other timelines don't update as frequently
            // and it just adds noise to the interface with
            // red dots on the other timelines.

            if (DataContext is HomeTimelineControlViewModel vm)
            {
                vm.IsScrolled = e.VerticalOffset > 0;
            }
        }
    }
}