using System;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class UserProfileBlockViewModel : NotifyPropertyChanged
    {
        public UserProfileBlockViewModel(ITwitterService twitterService, ISettings settings)
        {
            TwitterService = twitterService;
            Settings = settings;
        }

        private User? user;
        private string? errorMessage;
        private ITwitterService TwitterService { get; }
        public ISettings Settings { get; }

        public User? User { get => user; set => SetProperty(ref user, value); }
        public string? ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }

        public async Task GetUserInfo(string? screenName)
        {
            try
            {
                // UI reuse in virtual panels can show old values until updated.
                User = null;
                ErrorMessage = null;

                if (screenName == null)
                {
                    throw new ArgumentNullException(nameof(GetUserInfo), nameof(screenName));
                }

                User = await TwitterService.UserInfo(screenName);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}