using System.Threading.Tasks;
using twitter.core.Services;

namespace tweetz.core.Interfaces
{
    public interface ITwitterService
    {
        TwitterApi TwitterApi { get; }
        ValueTask AuthenticateWithPinAsync(OAuthTokens requestTokens, string pin);
    }
}