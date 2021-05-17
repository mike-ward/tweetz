using System;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using twitter.core.Services;

namespace tweetz.core.ViewModels
{
    public class GetPinControlViewModel : NotifyPropertyChanged
    {
        private          string?         pin;
        private          OAuthTokens?    requestToken;
        private readonly ITwitterService _twitterService;

        public GetPinControlViewModel(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        public string? Pin
        {
            get => pin;
            set => SetProperty(ref pin, value);
        }

        internal async ValueTask GetPinAsync()
        {
            requestToken = await _twitterService.GetPin().ConfigureAwait(false);
        }

        internal async ValueTask SignInAsync()
        {
            if (requestToken is null) throw new InvalidOperationException("requestToken is null");
            if (string.IsNullOrWhiteSpace(Pin)) throw new InvalidOperationException("Pin is null");

            await _twitterService.AuthenticateWithPinAsync(requestToken, Pin).ConfigureAwait(false);
            Pin = null;
        }
    }
}