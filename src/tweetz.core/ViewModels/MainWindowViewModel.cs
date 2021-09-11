using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.Services;

// ReSharper disable MemberCanBePrivate.Global

namespace tweetz.core.ViewModels
{
    public class MainWindowViewModel
    {
        public  ISettings                    Settings              { get; }
        private ISystemState                 SystemState           { get; }
        private IWindowInteropService        WindowInteropService  { get; }
        private ISystemTrayIconService       SystemTrayIconService { get; }
        private IEnumerable<ICommandBinding> CommandBindings       { get; }
        private IImageViewerService          ImageViewerService    { get; }

        public MainWindowViewModel(
            ISettings settings,
            ISystemState systemState,
            IImageViewerService imageViewerService,
            IWindowInteropService windowInteropService,
            ISystemTrayIconService systemTrayIconService,
            IEnumerable<ICommandBinding> commandBindings)
        {
            Settings              = settings;
            SystemState           = systemState;
            ImageViewerService    = imageViewerService;
            WindowInteropService  = windowInteropService;
            SystemTrayIconService = systemTrayIconService;
            CommandBindings       = commandBindings;
        }

        public void Initialize(Window window)
        {
            Settings.Load();
            SystemTrayIconService.Initialize(window);
            WindowInteropService.PowerManagementRegistration(window, SystemState);
            WindowInteropService.SetWindowPosition(window, Settings.MainWindowPosition);
            InitializeSaveSettingsOnMove(window);

            window.CommandBindings.AddRange(CommandBindings.Select(cb => cb.CommandBinding()).ToArray());
            window.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, delegate { window.Close(); }));
        }

        public void OnClosing(Window window)
        {
            SaveSettings(window);
            ImageViewerService.Close();
            SystemTrayIconService.Close();
        }

        private void InitializeSaveSettingsOnMove(Window window)
        {
            const int OneSecond    = 1000;
            var       saveSettings = DebounceService.Debounce<Window>(w => SaveSettings(w), OneSecond);
            window.SizeChanged     += delegate { saveSettings(window); };
            window.LocationChanged += delegate { saveSettings(window); };
        }

        private void SaveSettings(Window window)
        {
            Settings.MainWindowPosition = WindowInteropService.GetWindowPosition(window);
            Settings.Save();
        }
    }
}