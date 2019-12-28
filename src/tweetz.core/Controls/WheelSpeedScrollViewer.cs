using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace tweetz.core.Controls
{
    public class WheelSpeedScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty SpeedFactorProperty =
            DependencyProperty.Register(nameof(SpeedFactor),
                                        typeof(double),
                                        typeof(WheelSpeedScrollViewer),
                                        new PropertyMetadata(1.0));

        public double SpeedFactor
        {
            get { return (double)GetValue(SpeedFactorProperty); }
            set { SetValue(SpeedFactorProperty, value); }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (!e.Handled &&
                ScrollInfo is VirtualizingStackPanel vsp &&
                ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                vsp.SetVerticalOffset(VerticalOffset - e.Delta * SpeedFactor);
                e.Handled = true;
            }
        }
    };
}