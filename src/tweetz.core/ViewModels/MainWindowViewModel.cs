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
        private IEnumerable<ICommandBinding> CommandBindings { get; }

        public MainWindowViewModel(
            ISettings settings,
            ISystemState systemState,
            IWindowInteropService windowInteropService,
            IEnumerable<ICommandBinding> commandBindings)
        {
            Settings = settings;
            SystemState = systemState;
            WindowInteropService = windowInteropService;
            CommandBindings = commandBindings;
        }

        public void Initialize(Window window)
        {
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
        }
    }
}