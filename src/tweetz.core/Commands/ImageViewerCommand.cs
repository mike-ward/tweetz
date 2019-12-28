using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ImageViewerCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();
        private Popup? _popup;
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
            if (_popup != null) _popup.IsOpen = false;

            try
            {
                var uri =
                    ea.Parameter is Media media ? ImageViewerService.MediaSource(media) :
                    ea.Parameter is string url ? new Uri(url) :
                    null;

                if (uri == null || !(sender is Window window)) return;
                _popup = ImageViewerService.CreatePopup(window, uri);
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
        }
    }
}