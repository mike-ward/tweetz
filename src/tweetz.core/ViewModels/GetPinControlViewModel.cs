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
            requestToken = await _twitterService.GetPin();
        }

        internal async Task SignIn()
        {
            if (requestToken == null) throw new InvalidOperationException("requestToken can not be null");
            if (string.IsNullOrWhiteSpace(Pin)) throw new InvalidOperationException("Pin can not be null");

            await _twitterService.AuthenticateWithPin(requestToken, Pin);
            Pin = null;
        }
    }
}