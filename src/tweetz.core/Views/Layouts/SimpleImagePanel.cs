using System.Windows;
using System.Windows.Controls;

namespace tweetz.core.Views.Layouts
{
    internal class SimpleImagePanel : Panel
    {
        /// <summary>
        /// Limited image panel designed specifically to display the 1-4 images
        /// that twitter returns. Not at all generic.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(availableSize.Width, 200);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double w;
            double h;

            switch (Children.Count)
            {
                // fill enire panel
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

                default:
                    break;
            }

            return finalSize;
        }
    }
}