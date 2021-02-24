using System.Runtime.InteropServices;

namespace tweetz.core.DesktopWindowManager
{
    internal static class DesktopWindowManager
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(out bool enabled);

        public static bool IsDwmEnabled()
        {
            _ = DwmIsCompositionEnabled(out var enabled);
            return enabled;
        }
    }
}