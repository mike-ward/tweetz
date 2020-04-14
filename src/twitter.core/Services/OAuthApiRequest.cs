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
    internal class OAuthApiRequest
    {
        public const string GET = "GET";
        public const string POST = "POST";

        private string? ConsumerKey { get; set; }
        private string? ConsumerSecret { get; set; }
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
            return Request<T>(url, parameters, GET);
        }

        public ValueTask Post(string url, IEnumerable<(string, string)> parameters)
        {
            return Request(url, parameters, POST);
        }

        public ValueTask<T> Post<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return Request<T>(url, parameters, POST);
        }

        private ValueTask Request(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var _ = Request<string>(url, parameters, method);
            return default;
        }

        private async ValueTask<T> Request<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            return await OAuthRequest<T>(url, parameters, method).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds, signs and delivers an OAuth Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private async ValueTask<T> OAuthRequest<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var post = string.Equals(method, POST, StringComparison.Ordinal);
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var parray = parameters;
            var signature = OAuth.Signature(method, url, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, parray);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);
            var parameterStrings = parray.Select(p => $"{OAuth.UrlEncode(p.Item1)}={OAuth.UrlEncode(p.Item2)}").ToList();
            if (!post) url += $"?{string.Join("&", parameterStrings)}";

            var request = WebRequest.Create(url);
            request.Method = method;
            request.Headers.Add("Authorization", authorizeHeader);

            if (post)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                using var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
                await WriteTextToStream(requestStream, string.Join("&", parameterStrings)).ConfigureAwait(false);
            }

            using var response = await request.GetResponseAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<T>(response.GetResponseStream()).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Twitter requires media upload to be multipart form with specific parameters
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async ValueTask AppendMedia(string mediaId, int segmentIndex, byte[] payload)
        {
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var uploadUrl = TwitterApi.UploadMediaUrl;
            var signature = OAuth.Signature(POST, uploadUrl, nonce, timestamp, ConsumerKey!, ConsumerSecret!, AccessToken!, AccessTokenSecret!, null);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey!, AccessToken, signature);

            var request = WebRequest.Create(uploadUrl);
            request.Headers.Add("Authorization", authorizeHeader);
            request.Method = POST;

            var boundary = $"{Guid.NewGuid():N}";
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using var requestStream = await request.GetRequestStreamAsync();
            await TextParameter(requestStream, boundary, "command", "APPEND");
            await TextParameter(requestStream, boundary, "media_id", mediaId);
            await TextParameter(requestStream, boundary, "segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture));
            await BinaryParameter(requestStream, boundary, "media", payload);
            await WriteTextToStream(requestStream, $"--{boundary}--\r\n");

            using var _ = await request.GetResponseAsync().ConfigureAwait(false);
        }

        private static async ValueTask TextParameter(Stream stream, string boundary, string name, string payload)
        {
            var header = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n";
            await WriteTextToStream(stream, header);
            await WriteTextToStream(stream, payload);
            await WriteTextToStream(stream, "\r\n");
        }

        private static async ValueTask BinaryParameter(Stream stream, string boundary, string name, byte[] payload)
        {
            var header =
                $"--{boundary}\r\nContent-Type: application/octet-stream\r\n" +
                $"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n";

            await WriteTextToStream(stream, header).ConfigureAwait(false);
            await stream.WriteAsync(payload, 0, payload.Length).ConfigureAwait(false);
            await WriteTextToStream(stream, "\r\n").ConfigureAwait(false);
        }

        private static async ValueTask WriteTextToStream(Stream stream, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }
    }
}