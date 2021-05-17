using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.ViewModels;

namespace tweetz.core.Commands
{
    public class RemoveImageCommand : ICommandBinding
    {
        public static readonly RoutedCommand           Command = new RoutedUICommand();
        private                ComposeControlViewModel ComposeControlViewModel { get; }

        public RemoveImageCommand(ComposeControlViewModel composeControlViewModel)
        {
            ComposeControlViewModel = composeControlViewModel;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            ComposeControlViewModel.RemoveImage(ea.Parameter as string ?? string.Empty);
        }
    }
}