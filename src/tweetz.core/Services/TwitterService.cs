using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using twitter.core.Models;
using twitter.core.Services;

namespace tweetz.core.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly ISettings  settings;
        public  TwitterApi TwitterApi { get; }

        public TwitterService(ISettings settings)
        {
            if (settings is null) throw new System.ArgumentNullException(nameof(settings));

            const string consumerKey    = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";

            TwitterApi    = new TwitterApi(consumerKey, consumerSecret);
            this.settings = settings;

            this.settings.PropertyChanged += delegate
            {
                TwitterApi.AuthenticationTokens(
                    this.settings.AccessToken,
                    this.settings.AccessTokenSecret);
            };
        }

        public async ValueTask AuthenticateWithPinAsync(OAuthTokens requestTokens, string pin)
        {
            var access = await TwitterApi.AuthenticateWithPin(requestTokens, pin).ConfigureAwait(true);
            settings.AccessToken       = access.OAuthToken;
            settings.AccessTokenSecret = access.OAuthSecret;
            settings.ScreenName        = access.ScreenName;
            settings.Save();
        }
    }
}