using System.Windows.Input;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    public class IncreaseFontSizeCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        public IncreaseFontSizeCommand(ISettings settings)
        {
            Settings = settings;
        }

        public ISettings Settings { get; }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Settings.FontSize += 0.1;
        }
    }
}