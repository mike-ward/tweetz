using System.Runtime.InteropServices;

namespace tweetz.core.Infrastructure.DesktopWindowManager
{
    internal static class DesktopWindowManager
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        public static bool IsDwmEnabled()
        {
            DwmIsCompositionEnabled(out var enabled);
            return enabled;
        }
    }
}