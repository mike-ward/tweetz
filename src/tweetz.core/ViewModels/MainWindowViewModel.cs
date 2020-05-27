using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using tweetz.core.Infrastructure;

namespace tweetz.core.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public ISettings Settings { get; }
        public ISystemState SystemState { get; }
        private IWindowInteropService WindowInteropService { get; }
        public ISystemTrayIconService SystemTrayIconService { get; }
        private IEnumerable<ICommandBinding> CommandBindings { get; }
        public IImageViewerService ImageViewerService { get; }

        public MainWindowViewModel(
            ISettings settings,
            ISystemState systemState,
            IImageViewerService imageViewerService,
            IWindowInteropService windowInteropService,
            ISystemTrayIconService systemTrayIconService,
            IEnumerable<ICommandBinding> commandBindings)
        {
            Settings = settings;
            SystemState = systemState;
            WindowInteropService = windowInteropService;
            SystemTrayIconService = systemTrayIconService;
            CommandBindings = commandBindings;
            ImageViewerService = imageViewerService;
        }

        public void Initialize(Window window)
        {
            if (window is null) throw new System.ArgumentNullException(nameof(window));

            Settings.Load();
            SystemTrayIconService.Initialize(window);
            WindowInteropService.PowerManagementRegistration(window, SystemState);
            WindowInteropService.SetWindowPosition(window, Settings.MainWindowPosition);

            window.CommandBindings.AddRange(CommandBindings.Select(cb => cb.CommandBinding()).ToList());
            window.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (_, __) => window.Close()));
        }

        public void OnClosing(Window window)
        {
            Settings.MainWindowPosition = WindowInteropService.GetWindowPosition(window);
            Settings.Save();

            ImageViewerService.Close();
            SystemTrayIconService.Close();
        }
    }
}