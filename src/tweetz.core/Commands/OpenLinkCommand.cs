using System;
using System.Windows.Input;
using tweetz.core.Infrastructure;

namespace tweetz.core.Commands
{
    public class OpenLinkCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private IOpenUrlService OpenUrlService { get; }
        public IMessageBoxService MessageBoxService { get; }

        public OpenLinkCommand(IOpenUrlService openUrlService, IMessageBoxService messageBoxService)
        {
            OpenUrlService = openUrlService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            if (ea.Parameter is string url)
            {
                try
                {
                    OpenUrlService.OpenUrl(url);
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowMessageBox(ex.Message);
                }
            }
        }
    }
}