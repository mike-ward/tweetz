using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using tweetz.core.ViewModels;

namespace tweetz.core.Controls
{
    public partial class TimelineControl : UserControl
    {
        private static readonly ThicknessAnimation SlideDownAnimation =
            new ThicknessAnimation(new Thickness(0, -100, 0, 0), new Thickness(0), TimeSpan.FromMilliseconds(100));

        public TimelineControl()
        {
            InitializeComponent();
            Loaded += TimelineControl_Loaded;
        }

        private void TimelineControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeTimelineControlViewModel vm)
            {
                vm.StatusCollection.CollectionChanged += (s, args) =>
                {
                    if (!vm.IsScrolled)
                    {
                        ItemsControl.BeginAnimation(MarginProperty, SlideDownAnimation);
                    }
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