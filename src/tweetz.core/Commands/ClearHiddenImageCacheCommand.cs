using System;
using System.Windows;
using System.Windows.Input;
using tweetz.core.Interfaces;

namespace tweetz.core.Commands
{
    internal class ClearHiddenImageCacheCommand : ICommandBinding
    {
        public static readonly RoutedCommand      Command = new RoutedUICommand();
        private                ISettings          Settings          { get; }
        private                IMessageBoxService MessageBoxService { get; }

        public ClearHiddenImageCacheCommand(ISettings settings, IMessageBoxService messageBoxService)
        {
            Settings          = settings;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        private void CommandHandler(object sender, ExecutedRoutedEventArgs ea)
        {
            try
            {
                if (MessageBoxService.ShowMessageBoxYesNo(App.GetString("clear-image-cache-question")) == MessageBoxResult.Yes)
                {
                    Settings.HiddenImageSet.Clear();
                    Settings.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
        }
    }
}