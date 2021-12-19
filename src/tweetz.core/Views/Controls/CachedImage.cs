using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tweetz.core.Views.Controls
{
    public class Image : System.Windows.Controls.Image
    {
        private static RequestCachePolicy UriCachePolicy { get; } = new(RequestCacheLevel.Default);

        static Image()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Image),
                new FrameworkPropertyMetadata(typeof(Image)));
        }

        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(
                "ImageUrl",
                typeof(string),
                typeof(Image),
                new PropertyMetadata(string.Empty, ImageUrlPropertyChanged));

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(url)) return;

            var image       = (Image)obj;
            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.UriSource      = new Uri(url);
            bitmapImage.UriCachePolicy = UriCachePolicy;
            bitmapImage.CreateOptions  = BitmapCreateOptions.IgnoreColorProfile;
            bitmapImage.EndInit();
            image.Source = bitmapImage;
        }

        // WPF will obey the image DPI if it has one but does not adjust the
        // image size resulting in images being too small. MeasureOverride
        // and ArrangeOverride account for the embedded DPI when present.

        protected override Size MeasureOverride(Size constraint)
        {
            if (Source is not BitmapImage bitmapImage) return new Size(0, 0);

            var dpiScale    = GetDpiScale(this);
            var scaledSize  = new Size(bitmapImage.PixelWidth / dpiScale.Width, bitmapImage.PixelHeight / dpiScale.Height);
            var desiredSize = CalculateDesiredSize(scaledSize, constraint);

            if (UseLayoutRounding)
            {
                desiredSize.Width  = Math.Round(desiredSize.Width);
                desiredSize.Height = Math.Round(desiredSize.Height);
            }

            return double.IsInfinity(desiredSize.Width) || double.IsInfinity(desiredSize.Height)
                ? scaledSize
                : desiredSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return new Size(Math.Round(DesiredSize.Width), Math.Round(DesiredSize.Height));
        }

        private static Size GetDpiScale(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);
            if (source?.CompositionTarget is null) return new Size(1, 1);

            var dpiScale = new Size(
                source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22);

            return dpiScale;
        }

        private static Size CalculateDesiredSize(Size desiredSize, Size constraint)
        {
            var xRatio = constraint.Width / desiredSize.Width;
            var yRatio = constraint.Height / desiredSize.Height;
            var ratio  = Math.Min(xRatio, yRatio);
            return new Size(desiredSize.Width * ratio, desiredSize.Height * ratio);
        }
    }
}