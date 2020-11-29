using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class ToggleShowComposeCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private TabBarControlViewModel TabBarControlViewModel { get; }
        private ComposeControlViewModel ComposeControlViewModel { get; }

        public ToggleShowComposeCommand(TabBarControlViewModel tabBarControlViewModel, ComposeControlViewModel composeControlViewModel)
        {
            TabBarControlViewModel = tabBarControlViewModel;
            ComposeControlViewModel = composeControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            ComposeControlViewModel.Clear();
            TabBarControlViewModel.ShowComposeControl = !TabBarControlViewModel.ShowComposeControl;
        }
    }
}