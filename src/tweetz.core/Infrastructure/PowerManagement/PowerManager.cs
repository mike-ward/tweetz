using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace tweetz.core.Infrastructure.PowerManagement
{
    internal static class PowerManager
    {
        private static Guid MonitorPowerStatus = new Guid(0x02731015, 0x4510, 0x4526, 0x99, 0xe6, 0xe5, 0xa1, 0x7e, 0xbd, 0x1a, 0xea);

        public static void RegisterMonitorStatusChange(Window window)
        {
            PowerManagementNativeMethods.RegisterPowerSettingNotification(new WindowInteropHelper(window).Handle, ref MonitorPowerStatus, 0);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "none")]
        public static int MonitorStatus(IntPtr wParam, IntPtr lParam)
        {
#pragma warning disable CS8605 // Unboxing a possibly null value.
            var ps = (PowerManagementNativeMethods.PowerBroadcastSetting)Marshal
                .PtrToStructure(lParam, typeof(PowerManagementNativeMethods.PowerBroadcastSetting));
#pragma warning restore CS8605 // Unboxing a possibly null value.

            var pData = new IntPtr(lParam.ToInt64() + Marshal.SizeOf(ps));
            var monitorStatus = -1;

            if (ps.PowerSetting == MonitorPowerStatus && ps.DataLength == Marshal.SizeOf(typeof(int)))
            {
#pragma warning disable CS8605 // Unboxing a possibly null value.
                monitorStatus = (int)Marshal.PtrToStructure(pData, typeof(int));
#pragma warning restore CS8605 // Unboxing a possibly null value.
            }
            return monitorStatus;
        }
    }
}