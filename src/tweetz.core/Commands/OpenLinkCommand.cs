using System;
using System.Windows.Input;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    public class OpenLinkCommand : ICommandBinding
    {
        public static readonly RoutedCommand      Command = new RoutedUICommand();
        private                IOpenUrlService    OpenUrlService    { get; }
        private                IMessageBoxService MessageBoxService { get; }

        public OpenLinkCommand(IOpenUrlService openUrlService, IMessageBoxService messageBoxService)
        {
            OpenUrlService    = openUrlService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            {
                try
                {
                    if (ea?.Parameter is string url)
                        OpenUrlService.OpenUrl(url);
                    else
                        MessageBoxService.ShowMessageBox(ea?.Parameter as string);
                }
                catch (Exception ex)
                {
                    MessageBoxService.ShowMessageBox(ex.Message);
                }
            }
        }
    }
}