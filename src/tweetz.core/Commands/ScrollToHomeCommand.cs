using System.Windows.Input;
using tweetz.core.Controls;
using tweetz.core.Infrastructure;

namespace tweetz.core.Commands
{
    public class ScrollToHomeCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            var timeline = args.Parameter as TimelineControl;
            timeline?.ScrollToHome();
            args.Handled = false;
        }
    }
}