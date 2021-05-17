using System.Windows.Input;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    public class ToggleSpellCheckerCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private                ISettings     Settings { get; }

        public ToggleSpellCheckerCommand(ISettings settings)
        {
            Settings = settings;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            Settings.SpellCheck = !Settings.SpellCheck;
        }
    }
}