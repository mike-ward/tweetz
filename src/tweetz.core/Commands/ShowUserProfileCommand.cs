using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Commands
{
    public class ShowUserProfileCommand : ICommandBinding
    {
        private readonly       MainViewModel   mainViewModel;
        private readonly       ITwitterService twitterService;
        public static readonly RoutedCommand   Command = new RoutedUICommand();

        public ShowUserProfileCommand(MainViewModel mainViewModel, ITwitterService twitterService)
        {
            this.mainViewModel  = mainViewModel;
            this.twitterService = twitterService;
        }

        public CommandBinding CommandBinding()
        {
            return new CommandBinding(Command, CommandHandler);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void CommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                mainViewModel.UserProfile = e.Parameter switch {
                    User user         => mainViewModel.UserProfile = user,
                    string screenName => await UserInfo(screenName).ConfigureAwait(false),
                    _                 => null
                };
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private async ValueTask<User?> UserInfo(string screenName)
        {
            try
            {
                return await twitterService.TwitterApi.UserInfo(screenName).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}