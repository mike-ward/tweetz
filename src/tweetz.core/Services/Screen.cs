using System.Data;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace tweetz.core.Services
{
    public static class Screen
    {
        private static Matrix GetSizeFactors(Visual element)
        {
            var source = PresentationSource.FromVisual(element);
            if (source is not null)
            {
                return source.CompositionTarget?.TransformToDevice ?? throw new InvalidConstraintException("CompositionTarget is null");
            }

            using var source2 = new HwndSource(new HwndSourceParameters());
            return source2.CompositionTarget?.TransformToDevice ?? throw new InvalidConstraintException("CompositionTarget is null");
        }

        public static double HorizontalDpiToPixel(UIElement element, double x) => x * GetSizeFactors(element).M11;

        public static double VerticalDpiToPixel(UIElement element, double y) => y * GetSizeFactors(element).M22;

        public static double HorizontalPixelToDpi(UIElement element, double x) => x / GetSizeFactors(element).M11;

        public static double VerticalPixelToDpi(UIElement element, double y) => y / GetSizeFactors(element).M22;

        public static Rect ScreenRectFromWindow(Window window)
        {
            var size         = WpfScreen.GetScreenFrom(window).DisplaySize;
            var x            = HorizontalPixelToDpi(window, size.X);
            var y            = VerticalPixelToDpi(window, size.Y);
            var screenWidth  = HorizontalPixelToDpi(window, size.Width);
            var screenHeight = VerticalPixelToDpi(window, size.Height);
            return new Rect(x, y, screenWidth, screenHeight);
        }
    }
}