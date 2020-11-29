using System;
using System.Windows;
using System.Windows.Input;
using tweetz.core.Interfaces;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ImageViewerCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private IImageViewerService ImageViewerService { get; }

        private IMessageBoxService MessageBoxService { get; }

        public ImageViewerCommand(IImageViewerService imageViewerService, IMessageBoxService messageBoxService)
        {
            ImageViewerService = imageViewerService;
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
                var uri = ea.Parameter switch
                {
                    Media media => Services.ImageViewerService.MediaSource(media),
                    string path => new Uri(path),
                    _ => null
                };

                if (uri is null || !(sender is Window)) return;
                ImageViewerService.Open(uri);
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
        }
    }
}