using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace twitter.core.Services
{
    internal static class OAuth
    {
        static OAuth()
        {
            ServicePointManager.Expect100Continue = false;
        }

        public static string UrlEncode(string value)
        {
            var encoded = Uri.EscapeDataString(value);
            return Regex
                .Replace(encoded, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpperInvariant(), RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1))
                .Replace("(", "%28", StringComparison.Ordinal)
                .Replace(")", "%29", StringComparison.Ordinal)
                .Replace("$", "%24", StringComparison.Ordinal)
                .Replace("!", "%21", StringComparison.Ordinal)
                .Replace("*", "%2A", StringComparison.Ordinal)
                .Replace("'", "%27", StringComparison.Ordinal)
                .Replace("%7E", "~", StringComparison.Ordinal);
        }

        public static string Nonce()
        {
            return Guid.NewGuid().ToString();
        }

        public static string TimeStamp()
        {
            var timespan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result   = Convert.ToInt64(timespan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
            return result;
        }

        public static string Signature(
            string httpMethod,
            string url,
            string nonce,
            string timestamp,
            string consumerKey,
            string consumerSecret,
            string accessToken,
            string accessTokenSecret,
            IEnumerable<(string, string)>? parameters)
        {
            var       parameterList       = OrderedParameters(nonce, timestamp, consumerKey, accessToken, signature: null, parameters);
            var       parameterStrings    = parameterList.Select(p => $"{p.Item1}={p.Item2}");
            var       parameterString     = string.Join("&", parameterStrings);
            var       signatureBaseString = $"{httpMethod}&{UrlEncode(url)}&{UrlEncode(parameterString)}";
            var       compositeKey        = $"{UrlEncode(consumerSecret)}&{UrlEncode(accessTokenSecret)}";
            using var hmac                = new HMACSHA1(Encoding.UTF8.GetBytes(compositeKey));
            var       result              = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBaseString)));
            return result;
        }

        public static string AuthorizationHeader(
            string nonce,
            string timestamp,
            string consumerKey,
            string? accessToken,
            string? signature,
            IEnumerable<(string, string)>? parameters = null)
        {
            var parameterList    = OrderedParameters(nonce, timestamp, consumerKey, accessToken, signature, parameters);
            var parameterStrings = parameterList.Select(p => $"{p.Item1}=\"{p.Item2}\"");
            var header           = "OAuth " + string.Join(",", parameterStrings);
            return header;
        }

        private static IEnumerable<(string, string)> OrderedParameters(
            string nonce,
            string timestamp,
            string consumerKey,
            string? accessToken,
            string? signature,
            IEnumerable<(string, string)>? parameters)
        {
            return
                Parameters(nonce, timestamp, consumerKey, accessToken, signature, parameters)
                    .OrderBy(p => p.Item1, StringComparer.Ordinal);
        }

        private static IEnumerable<(string, string)> Parameters(
            string nonce,
            string timestamp,
            string consumerKey,
            string? accessToken,
            string? signature,
            IEnumerable<(string, string)>? parameters)
        {
            yield return ("oauth_version", "1.0");
            yield return ("oauth_nonce", UrlEncode(nonce));
            yield return ("oauth_timestamp", UrlEncode(timestamp));
            yield return ("oauth_signature_method", "HMAC-SHA1");
            yield return ("oauth_consumer_key", UrlEncode(consumerKey));

            if (!string.IsNullOrWhiteSpace(signature))
            {
                yield return ("oauth_signature", UrlEncode(signature));
            }

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                yield return ("oauth_token", UrlEncode(accessToken));
            }

            if (parameters is not null)
            {
                foreach (var par in parameters.Select(par => (UrlEncode(par.Item1), UrlEncode(par.Item2))))
                {
                    yield return par;
                }
            }
        }
    }
}