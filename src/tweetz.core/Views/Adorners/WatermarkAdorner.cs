using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace tweetz.core.Views.Adorners
{
    internal sealed class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark) :
           base(adornedElement)
        {
            IsHitTestVisible = false;

            contentPresenter = new ContentPresenter
            {
                Content = watermark,
                Opacity = 0.5,
                Margin = new Thickness(Control.Margin.Left + Control.Padding.Left, Control.Margin.Top + Control.Padding.Top, 0, 0),
            };

            if (Control is ItemsControl && !(Control is ComboBox))
            {
                contentPresenter.VerticalAlignment = VerticalAlignment.Center;
                contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            }

            var binding = new Binding("IsVisible")
            {
                Source = adornedElement,
                Converter = new BooleanToVisibilityConverter(),
            };
            SetBinding(VisibilityProperty, binding);
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        private Control Control
        {
            get { return (Control)AdornedElement; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            // Here's the secret to getting the adorner to cover the whole control
            contentPresenter.Measure(Control.RenderSize);
            return Control.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}