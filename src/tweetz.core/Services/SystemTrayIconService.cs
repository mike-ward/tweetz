using System.Windows;
using System.Windows.Forms;
using tweetz.core.Infrastructure;

namespace tweetz.core.Services
{
    public class SystemTrayIconService : ISystemTrayIconService
    {
        private NotifyIcon NotifyIcon { get; }
        private ISettings Settings { get; }

        public SystemTrayIconService(ISettings settings)
        {
            Settings = settings;
            NotifyIcon = new NotifyIcon();
        }

        public void InitializeSystemTrayIcon(Window window)
        {
            NotifyIcon.Text = (string)System.Windows.Application.Current.FindResource("title");
            NotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly()?.ManifestModule.Name);

            NotifyIcon.Click += (_, __) =>
            {
                // Bring window to front
                window.WindowState = WindowState.Minimized;
                window.Show();
                window.WindowState = WindowState.Normal;
            };

            Settings.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(Settings.ShowInSystemTray))
                {
                    window.ShowInTaskbar = !Settings.ShowInSystemTray;
                    NotifyIcon.Visible = Settings.ShowInSystemTray;
                }
            };
        }

        public void HideSystemTrayIcon()
        {
            NotifyIcon.Visible = false;
        }
    }
}