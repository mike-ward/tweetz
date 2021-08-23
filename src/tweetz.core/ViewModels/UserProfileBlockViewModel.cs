using System;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class UserProfileBlockViewModel : NotifyPropertyChanged
    {
        public UserProfileBlockViewModel(ITwitterService twitterService, ISettings settings)
        {
            TwitterService = twitterService;
            Settings       = settings;
        }

        private User?           user;
        private string?         errorMessage;
        private ITwitterService TwitterService { get; }
        public  ISettings       Settings       { get; }

        public User? User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public string? ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public async ValueTask GetUserInfoAsync(string? screenName)
        {
            try
            {
                // UI reuse in virtual panels can show old values until updated.
                User         = null;
                ErrorMessage = null;
                if (screenName is null)
                {
                    ErrorMessage = "screenName is null in method GetUserInfo";
                    return;
                }

                User = await TwitterService.TwitterApi.UserInfo(screenName).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
                ErrorMessage = ex.Message;
            }
        }
    }
}