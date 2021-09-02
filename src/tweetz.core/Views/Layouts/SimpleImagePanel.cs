using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Views.Layouts
{
    /// <summary>
    /// Limited image panel designed specifically to display the 1-4 images
    /// that twitter returns. Not generic.
    /// </summary>
    internal class SimpleImagePanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            switch (Children.Count)
            {
                case 1:
                    Children[0].Measure(availableSize);
                    return new Size(availableSize.Width, Math.Min(availableSize.Height, Children[0].DesiredSize.Height));

                case 2:
                {
                    var size = new Size(availableSize.Width / 2, availableSize.Height);
                    Children[0].Measure(size);
                    Children[1].Measure(size);
                    var desiredHeight = Math.Max(Children[0].DesiredSize.Height, Children[1].DesiredSize.Height);
                    return new Size(availableSize.Width, Math.Min(availableSize.Height, desiredHeight));
                }
                case 3:
                {
                    var size = new Size(availableSize.Width / 2, availableSize.Height);
                    Children[0].Measure(size);
                    if (Children[0].DesiredSize.Height >= availableSize.Height) return availableSize;

                    size = new Size(size.Width, size.Height / 2);
                    Children[1].Measure(size);
                    Children[2].Measure(size);
                    var desiredHeight = Math.Max(Children[0].DesiredSize.Height, Children[1].DesiredSize.Height + Children[2].DesiredSize.Height);
                    return new Size(availableSize.Width, Math.Min(availableSize.Height, desiredHeight));
                }
                case 4:
                {
                    var size = new Size(availableSize.Width / 2, availableSize.Height / 2);
                    Children[0].Measure(size);
                    Children[1].Measure(size);
                    Children[2].Measure(size);
                    Children[3].Measure(size);
                    var desiredHeight = new[] {
                        Children[0].DesiredSize.Height,
                        Children[1].DesiredSize.Height,
                        Children[2].DesiredSize.Height,
                        Children[3].DesiredSize.Height
                    }.Max();
                    return new Size(availableSize.Width, Math.Min(availableSize.Height, desiredHeight * 2));
                }

                default:
                    return availableSize;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double w;
            double h;

            switch (Children.Count)
            {
                // fill entire panel
                case 1:
                    w = finalSize.Width;
                    h = finalSize.Height;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    break;

                // Two columns, side by side
                case 2:
                    w = finalSize.Width / 2;
                    h = finalSize.Height;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    break;

                // Two columns, Two row, first column spans both rows
                case 3:
                    w = finalSize.Width / 2;
                    h = finalSize.Height / 2;
                    Children[0].Arrange(new Rect(0, 0, w, finalSize.Height));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    Children[2].Arrange(new Rect(w, h, w, h));
                    break;

                // Two rows, two columns
                case 4:
                    w = finalSize.Width / 2;
                    h = finalSize.Height / 2;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    Children[2].Arrange(new Rect(0, h, w, h));
                    Children[3].Arrange(new Rect(w, h, w, h));
                    break;
            }

            return finalSize;
        }
    }
}