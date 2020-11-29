using System.Windows.Input;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    public class SignOutCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        public SignOutCommand(ISettings settings)
        {
            Settings = settings;
        }

        public ISettings Settings { get; }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            Settings.AccessToken = null;
            Settings.AccessTokenSecret = null;
        }
    }
}