using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace twitter.core.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "None")]
    internal static class TwitterTokenRequest
    {
        public static async ValueTask<OAuthTokens> GetRequestToken(
            string consumerKey,
            string consumerSecret)
        {
            const string requestTokenUrl = "https://api.twitter.com/oauth/request_token";
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var parameters = new[] { ("oauth_callback", "oob") };
            var signature = OAuth.Signature(OAuthApiRequest.POST, requestTokenUrl, nonce, timestamp, consumerKey, consumerSecret, "", "", parameters);
            var authorizationHeader = OAuth.AuthorizationHeader(nonce, timestamp, consumerKey, null, signature, parameters);

            var request = System.Net.WebRequest.Create(new Uri(requestTokenUrl));
            request.Method = OAuthApiRequest.POST;
            request.Headers.Add("Authorization", authorizationHeader);
            using var response = await request.GetResponseAsync().ConfigureAwait(false);
            using var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var body = await stream.ReadToEndAsync();
            var tokens = body.Split('&');
            var oauthToken = Token(tokens[0]);
            var oauthSecret = Token(tokens[1]);
            var callbackConfirmed = Token(tokens[2]);

            if (callbackConfirmed != "true")
            {
                throw new InvalidProgramException("callback token not confirmed");
            }

            return new OAuthTokens
            {
                OAuthToken = oauthToken,
                OAuthSecret = oauthSecret
            };
        }

        public static async ValueTask<OAuthTokens> GetAccessToken(
            string consumerKey,
            string consumerSecret,
            string accessToken,
            string accessTokenSecret,
            string oauthVerifier)
        {
            const string requestTokenUrl = "https://api.twitter.com/oauth/access_token";
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var parameters = new[] { ("oauth_verifier", oauthVerifier) };
            var signature = OAuth.Signature(OAuthApiRequest.POST, requestTokenUrl, nonce, timestamp, consumerKey, consumerSecret, accessToken, accessTokenSecret, parameters);
            var authorizationHeader = OAuth.AuthorizationHeader(nonce, timestamp, consumerKey, accessToken, signature, parameters);

            var request = System.Net.WebRequest.Create(new Uri(requestTokenUrl));
            request.Method = OAuthApiRequest.POST;
            request.Headers.Add("Authorization", authorizationHeader);

            using var response = await request.GetResponseAsync().ConfigureAwait(false);
            using var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var tokens = (await stream.ReadToEndAsync()).Split('&');

            var oauthTokens = new OAuthTokens
            {
                OAuthToken = Token(tokens[0]),
                OAuthSecret = Token(tokens[1]),
                UserId = Token(tokens[2]),
                ScreenName = Token(tokens[3])
            };

            return oauthTokens;
        }

        private static string Token(string pair)
        {
            return pair.Substring(pair.IndexOf('=', StringComparison.Ordinal) + 1);
        }
    }
}