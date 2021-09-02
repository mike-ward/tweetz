using System;
using System.Windows;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    internal class HideImageCommand : ICommandBinding
    {
        public static readonly RoutedCommand      Command = new RoutedUICommand();
        private                ISettings          Settings          { get; }
        private                IMessageBoxService MessageBoxService { get; }

        public HideImageCommand(ISettings settings, IMessageBoxService messageBoxService)
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
            ea.Handled = true;

            try
            {
                var uri = ea.Parameter switch {
                    Media media => ImageViewerService.MediaSource(media),
                    string path => new Uri(path),
                    _           => null
                };

                if (uri is not null && sender is Window)
                {
                    if (Settings.HiddenImageSet.Add(uri.ToString()))
                    {
                        Settings.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
        }
    }
}