using System;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Services;

namespace tweetz.core.ViewModels
{
    public class GetPinControlViewModel : NotifyPropertyChanged
    {
        private string? pin;
        private OAuthTokens? requestToken;
        private readonly ITwitterService _twitterService;

        public GetPinControlViewModel(ITwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        public string? Pin
        {
            get { return pin; }
            set { SetProperty(ref pin, value); }
        }

        internal async Task GetPin()
        {
            requestToken = await _twitterService.GetPin().ConfigureAwait(false);
        }

        internal async Task SignIn()
        {
            if (requestToken is null) throw new InvalidOperationException("requestToken is null");
            if (string.IsNullOrWhiteSpace(Pin)) throw new InvalidOperationException("Pin is null");

            await _twitterService.AuthenticateWithPin(requestToken, Pin).ConfigureAwait(false);
            Pin = null;
        }
    }
}