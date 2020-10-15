using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace twitter.core.Services
{
    internal sealed class OAuthApiRequest
    {
        public const string GET = "GET";
        public const string POST = "POST";

        private string? ConsumerKey { get; }
        private string? ConsumerSecret { get; }
        private string? AccessToken { get; set; }
        private string? AccessTokenSecret { get; set; }

        public OAuthApiRequest(string? consumerKey, string? consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public void AuthenticationTokens(string? accessToken, string? accessTokenSecret)
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public ValueTask Get(string url, IEnumerable<(string, string)> parameters)
        {
            return Request(url, parameters, GET);
        }

        public ValueTask<T> Get<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return RequestAsync<T>(url, parameters, GET);
        }

        public ValueTask Post(string url, IEnumerable<(string, string)> parameters)
        {
            return Request(url, parameters, POST);
        }

        public ValueTask<T> Post<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return RequestAsync<T>(url, parameters, POST);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage")]
        private async ValueTask Request(string url, IEnumerable<(string, string)> parameters, string method)
        {
            await RequestAsync<string>(url, parameters, method).ConfigureAwait(false);
        }

        private ValueTask<T> RequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            return OAuthRequestAsync<T>(url, parameters, method);
        }

        /// <summary>
        /// Builds, signs and delivers an OAuth Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private async ValueTask<T> OAuthRequestAsync<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var post = string.Equals(method, POST, StringComparison.Ordinal);
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var parray = parameters.ToArray();
            var signature = OAuth.Signature(method, url, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parray);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);
            var parameterStrings = parray.Select(p => $"{OAuth.UrlEncode(p.Item1)}={OAuth.UrlEncode(p.Item2)}");
            if (!post) url += $"?{string.Join("&", parameterStrings)}";

            var request = WebRequest.Create(url);
            request.Method = method;
            request.Headers.Add("Authorization", authorizeHeader);

            if (post)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                using var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
                await WriteTextToStreamAsync(requestStream, string.Join("&", parameterStrings)).ConfigureAwait(false);
            }

            using var response = await request.GetResponseAsync().ConfigureAwait(false);
            using var stream = response.GetResponseStream();
            var result = await JsonSerializer.DeserializeAsync<T>(stream).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("JsonSerializer.DeserializeAsync<T>(stream) return null");
        }

        /// <summary>
        /// Twitter requires media upload to be multipart form with specific parameters
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async ValueTask AppendMediaAsync(string mediaId, int segmentIndex, byte[] payload)
        {
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            const string uploadUrl = TwitterApi.UploadMediaUrl;
            var signature = OAuth.Signature(POST, uploadUrl, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parameters: null);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);

            var request = WebRequest.Create(uploadUrl);
            request.Headers.Add("Authorization", authorizeHeader);
            request.Method = POST;

            var boundary = $"{Guid.NewGuid():N}";
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "command", "APPEND").ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "media_id", mediaId).ConfigureAwait(false);
            await TextParameterAsync(requestStream, boundary, "segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture)).ConfigureAwait(false);
            await BinaryParameterAsync(requestStream, boundary, "media", payload).ConfigureAwait(false);
            await WriteTextToStreamAsync(requestStream, $"--{boundary}--\r\n").ConfigureAwait(false);

            using var _ = await request.GetResponseAsync().ConfigureAwait(false);
        }

        private static async ValueTask TextParameterAsync(Stream stream, string boundary, string name, string payload)
        {
            var header = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n";
            await WriteTextToStreamAsync(stream, header).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, payload).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, "\r\n").ConfigureAwait(false);
        }

        private static async ValueTask BinaryParameterAsync(Stream stream, string boundary, string name, byte[] payload)
        {
            var header =
                $"--{boundary}\r\nContent-Type: application/octet-stream\r\n" +
                $"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n";

            await WriteTextToStreamAsync(stream, header).ConfigureAwait(false);
            await stream.WriteAsync(payload.AsMemory(0, payload.Length)).ConfigureAwait(false);
            await WriteTextToStreamAsync(stream, "\r\n").ConfigureAwait(false);
        }

        private static ValueTask WriteTextToStreamAsync(Stream stream, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            return stream.WriteAsync(buffer.AsMemory(0, buffer.Length));
        }
    }
}