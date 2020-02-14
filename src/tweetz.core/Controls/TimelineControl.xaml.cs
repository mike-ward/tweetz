using System;
using System.Threading.Tasks;
using System.Windows;
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
            Loaded += TimelineControl_Loaded;
        }

        private void TimelineControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeTimelineControlViewModel vm)
            {
                vm.StatusCollection.CollectionChanged += async (s, args) =>
                {
                    const int duration = 1000;

                    // Only animate when scrolled to top
                    vm.FadeInDuration = vm.IsScrolled
                        ? TimeSpan.Zero
                        : TimeSpan.FromMilliseconds(duration);

                    await Task.Delay(duration + 200);
                    vm.FadeInDuration = TimeSpan.Zero;
                };
            }
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
            if (DataContext is HomeTimelineControlViewModel vm)
            {
                vm.IsScrolled = e.VerticalOffset > 0;
            }
        }
    }
}