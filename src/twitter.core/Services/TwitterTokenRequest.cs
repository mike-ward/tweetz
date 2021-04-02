using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace twitter.core.Services
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "None")]
    internal static class TwitterTokenRequest
    {
        public static async ValueTask<OAuthTokens> GetRequestTokenAsync(
            string consumerKey,
            string consumerSecret)
        {
            const string requestTokenUrl     = "https://api.twitter.com/oauth/request_token";
            var          nonce               = OAuth.Nonce();
            var          timestamp           = OAuth.TimeStamp();
            var          parameters          = new[] { ("oauth_callback", "oob") };
            var          signature           = OAuth.Signature(OAuthApiRequest.POST, requestTokenUrl, nonce, timestamp, consumerKey, consumerSecret, "", "", parameters);
            var          authorizationHeader = OAuth.AuthorizationHeader(nonce, timestamp, consumerKey, accessToken: null, signature, parameters);

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(requestTokenUrl));
            request.Headers.Add("Authorization", authorizationHeader);
            using var response = await OAuthApiRequest.MyHttpClient.SendAsync(request).ConfigureAwait(false);

            var body              = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var tokens            = body.Split('&');
            var oauthToken        = Token(tokens[0]);
            var oauthSecret       = Token(tokens[1]);
            var callbackConfirmed = Token(tokens[2]);

            if (string.CompareOrdinal(callbackConfirmed, "true") != 0)
            {
                throw new InvalidOperationException("callback token not confirmed");
            }

            return new OAuthTokens {
                OAuthToken  = oauthToken,
                OAuthSecret = oauthSecret
            };
        }

        public static async ValueTask<OAuthTokens> GetAccessTokenAsync(
            string consumerKey,
            string consumerSecret,
            string accessToken,
            string accessTokenSecret,
            string oauthVerifier)
        {
            const string requestTokenUrl     = "https://api.twitter.com/oauth/access_token";
            var          nonce               = OAuth.Nonce();
            var          timestamp           = OAuth.TimeStamp();
            var          parameters          = new[] { ("oauth_verifier", oauthVerifier) };
            var          signature           = OAuth.Signature(OAuthApiRequest.POST, requestTokenUrl, nonce, timestamp, consumerKey, consumerSecret, accessToken, accessTokenSecret, parameters);
            var          authorizationHeader = OAuth.AuthorizationHeader(nonce, timestamp, consumerKey, accessToken, signature, parameters);

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(requestTokenUrl));
            request.Headers.Add("Authorization", authorizationHeader);

            using var response = await OAuthApiRequest.MyHttpClient.SendAsync(request).ConfigureAwait(false);
            var       content  = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var       tokens   = content.Split('&');

            var oauthTokens = new OAuthTokens {
                OAuthToken  = Token(tokens[0]),
                OAuthSecret = Token(tokens[1]),
                UserId      = Token(tokens[2]),
                ScreenName  = Token(tokens[3])
            };

            return oauthTokens;
        }

        private static string Token(string pair)
        {
            return pair[(pair.IndexOf('=', StringComparison.Ordinal) + 1)..];
        }
    }
}