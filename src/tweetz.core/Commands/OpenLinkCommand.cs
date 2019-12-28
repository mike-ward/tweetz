using System.Windows.Input;
using tweetz.core.Infrastructure;

namespace tweetz.core.Commands
{
    public class OpenLinkCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private IOpenUrlService OpenUrlService { get; }

        public OpenLinkCommand(IOpenUrlService openUrlService)
        {
            OpenUrlService = openUrlService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            if (ea.Parameter is string url)
            {
                OpenUrlService.OpenUrl(url);
            }
        }
    }
}