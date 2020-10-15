using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using tweetz.core.Infrastructure;
using tweetz.core.Infrastructure.PowerManagement;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public sealed class WindowInteropService : IWindowInteropService
    {
        public void SetWindowPosition(Window window, WindowPosition windowPosition)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;
            var placement = ToWindowPlacement(windowPosition);
            SetWindowPlacement(windowHandle, ref placement);
        }

        public WindowPosition GetWindowPosition(Window window)
        {
            var windowHandle = new WindowInteropHelper(window).Handle;
            GetWindowPlacement(windowHandle, out var placement);
            return ToWindowPosition(placement);
        }

        private static WINDOWPLACEMENT ToWindowPlacement(WindowPosition position)
        {
            const int SW_SHOWNORMAL = 1;

            return new WINDOWPLACEMENT
            {
                length = Marshal.SizeOf(typeof(WINDOWPLACEMENT)),
                flags = 0,
                showCmd = SW_SHOWNORMAL,
                minPosition = new POINT { X = -1, Y = -1 },
                maxPosition = new POINT { X = -1, Y = -1 },
                normalPosition = new RECT
                {
                    Left = position.Left,
                    Top = position.Top,
                    Right = position.Left + position.Width,
                    Bottom = position.Top + position.Height,
                },
            };
        }

        private static WindowPosition ToWindowPosition(WINDOWPLACEMENT placement)
        {
            var pos = new WindowPosition
            {
                Left = placement.normalPosition.Left,
                Top = placement.normalPosition.Top,
                Width = placement.normalPosition.Right - placement.normalPosition.Left,
                Height = placement.normalPosition.Bottom - placement.normalPosition.Top,
            };

            return pos;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase")]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINT minPosition;
            public POINT maxPosition;
            public RECT normalPosition;
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        // Power Management - detect when monitor is sleeping

        private ISystemState? SystemState { get; set; }

        public void PowerManagementRegistration(Window window, ISystemState systemState)
        {
            SystemState = systemState;
            PowerManager.RegisterMonitorStatusChange(window);
            HwndSource.FromHwnd(new WindowInteropHelper(window).Handle).AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case (int)PowerManagementNativeMethods.PowerBroadcastMessage:
                    if ((int)wParam == PowerManagementNativeMethods.PowerSettingChangeMessage)
                    {
                        var monitorStatus = PowerManager.MonitorStatus(wParam, lParam);
                        if (SystemState != null)
                        {
                            SystemState.IsSleeping = monitorStatus == 0;
                        }
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}