using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using tweetz.core.Infrastructure;

namespace tweetz.core.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public ISettings Settings { get; }
        public ISystemState SystemState { get; }
        private IWindowInteropService WindowInteropService { get; }
        private IEnumerable<ICommandBinding> CommandBindings { get; }
        public IImageViewerService ImageViewerService { get; }
        private NotifyIcon notifyIcon;

        public MainWindowViewModel(
            ISettings settings,
            ISystemState systemState,
            IImageViewerService imageViewerService,
            IWindowInteropService windowInteropService,
            IEnumerable<ICommandBinding> commandBindings)
        {
            Settings = settings;
            SystemState = systemState;
            WindowInteropService = windowInteropService;
            CommandBindings = commandBindings;
            ImageViewerService = imageViewerService;
        }

        public void Initialize(Window window)
        {
            if (window is null) throw new System.ArgumentNullException(nameof(window));
            SystemTrayIcon(window);

            Settings.Load();
            WindowInteropService.PowerManagmentRegistration(window, SystemState);
            WindowInteropService.SetWindowPosition(window, Settings.MainWindowPosition);

            window.CommandBindings.AddRange(CommandBindings.Select(cb => cb.CommandBinding()).ToList());
            window.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, a) => window.Close()));
        }

        public void OnClosing(Window window)
        {
            Settings.MainWindowPosition = WindowInteropService.GetWindowPosition(window);
            Settings.Save();

            notifyIcon.Visible = false;
            ImageViewerService.Close();
        }

        private void SystemTrayIcon(Window window)
        {
            notifyIcon = new NotifyIcon
            {
                Text = (string)System.Windows.Application.Current.FindResource("title"),
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly()?.ManifestModule.Name),
            };

            notifyIcon.Click += (_, __) =>
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
                    notifyIcon.Visible = Settings.ShowInSystemTray;
                }
            };
        }
    }
}