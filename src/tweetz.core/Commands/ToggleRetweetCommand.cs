using System;
using System.Windows.Input;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ToggleRetweetCommand : ICommandBinding
    {
        public static readonly RoutedCommand Command = new RoutedUICommand();

        private ISettings Settings { get; }
        private ITwitterService TwitterService { get; }
        private IMessageBoxService MessageBoxService { get; }

        private bool inCommand;

        public ToggleRetweetCommand(
            ISettings settings,
            ITwitterService twitterService, IMessageBoxService messageBoxService)
        {
            Settings = settings;
            TwitterService = twitterService;
            MessageBoxService = messageBoxService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler, CanExecute);
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is TwitterStatus twitterStatus && twitterStatus.OriginatingStatus.User.ScreenName != Settings.ScreenName;
        }

        private async void CommandHandler(object sender, ExecutedRoutedEventArgs args)
        {
            if (inCommand) return;

            try
            {
                inCommand = true;

                if (args.Parameter is TwitterStatus twitterStatus)
                {
                    if (twitterStatus.RetweetedByMe)
                    {
                        await TwitterService.UnretweetStatus(twitterStatus.Id);
                        twitterStatus.RetweetCount = Math.Max(0, twitterStatus.RetweetCount - 1);
                        twitterStatus.RetweetedByMe = false;
                    }
                    else
                    {
                        await TwitterService.RetweetStatus(twitterStatus.Id);
                        twitterStatus.RetweetCount += 1;
                        twitterStatus.RetweetedByMe = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessageBox(ex.Message);
            }
            finally
            {
                inCommand = false;
            }
        }
    }
}