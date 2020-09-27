using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace tweetz.core.Views
{
    public class WheelSpeedScrollViewer : ScrollViewer
    {
        public double SpeedFactor
        {
            get => (double)GetValue(SpeedFactorProperty);
            set => SetValue(SpeedFactorProperty, value);
        }

        public static readonly DependencyProperty SpeedFactorProperty =
            DependencyProperty.Register(
                nameof(SpeedFactor),
                typeof(double),
                typeof(WheelSpeedScrollViewer),
                new PropertyMetadata(1.0));

        public TimeSpan ScrollDuration
        {
            get { return (TimeSpan)GetValue(ScrollDurationProperty); }
            set { SetValue(ScrollDurationProperty, value); }
        }

        public static readonly DependencyProperty ScrollDurationProperty =
            DependencyProperty.Register(
                nameof(ScrollDuration),
                typeof(TimeSpan),
                typeof(WheelSpeedScrollViewer),
                new PropertyMetadata(TimeSpan.FromMilliseconds(50)));

        public double VerticalOffsetMediator
        {
            get { return (double)GetValue(VerticalOffsetMediatorProperty); }
            set { SetValue(VerticalOffsetMediatorProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetMediatorProperty =
            DependencyProperty.Register(
                nameof(VerticalOffsetMediator),
                typeof(double),
                typeof(WheelSpeedScrollViewer),
                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is WheelSpeedScrollViewer scrollViewer &&
                scrollViewer.ScrollInfo is VirtualizingStackPanel virtualizingStackPanel &&
                scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                virtualizingStackPanel.SetVerticalOffset((double)e.NewValue);
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            AnimateScroll(VerticalOffset - (e.Delta * SpeedFactor));
            e.Handled = true;
        }

        private void AnimateScroll(double offset)
        {
            var animation = new DoubleAnimation
            {
                To = offset,
                Duration = ScrollDuration
            };

            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath(VerticalOffsetMediatorProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin(this, HandoffBehavior.Compose);
        }
    }
}