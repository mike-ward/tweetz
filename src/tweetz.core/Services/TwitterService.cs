using System;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using twitter.core.Services;

namespace tweetz.core.Services
{
    public class TwitterService : ITwitterService
    {
        private ISettings  Settings   { get; }
        public  TwitterApi TwitterApi { get; }

        public TwitterService(ISettings settings)
        {
            const string consumerKey    = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";

            Settings   = settings ?? throw new ArgumentNullException(nameof(settings));
            TwitterApi = new TwitterApi(consumerKey, consumerSecret);

            Settings.PropertyChanged += delegate
            {
                TwitterApi.AuthenticationTokens(
                    Settings.AccessToken,
                    Settings.AccessTokenSecret);
            };
        }

        public async ValueTask AuthenticateWithPinAsync(OAuthTokens requestTokens, string pin)
        {
            var access = await TwitterApi.AuthenticateWithPin(requestTokens, pin).ConfigureAwait(true);
            Settings.AccessToken       = access.OAuthToken;
            Settings.AccessTokenSecret = access.OAuthSecret;
            Settings.ScreenName        = access.ScreenName;
            Settings.Save();
        }
    }
}