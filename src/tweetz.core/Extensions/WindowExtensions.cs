using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Point = System.Drawing.Point;

namespace tweetz.core.Extensions
{
    public static class WindowExtensions
    {
        public static Rect GetCurrentScreenWorkArea(this Window window)
        {
            var screen   = Screen.FromPoint(new Point((int)window.Left, (int)window.Top));
            var dpiScale = VisualTreeHelper.GetDpi(window);

            return new Rect { Width = screen.WorkingArea.Width / dpiScale.DpiScaleX, Height = screen.WorkingArea.Height / dpiScale.DpiScaleY };
        }
    }
}