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

        public Task Get(string url, IEnumerable<(string, string)> parameters)
        {
            return Request(url, parameters, GET);
        }

        public Task<T> Get<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return Request<T>(url, parameters, GET);
        }

        public Task Post(string url, IEnumerable<(string, string)> parameters)
        {
            return Request(url, parameters, POST);
        }

        public Task<T> Post<T>(string url, IEnumerable<(string, string)> parameters)
        {
            return Request<T>(url, parameters, POST);
        }

        private Task Request(string url, IEnumerable<(string, string)> parameters, string method)
        {
            return OAuthRequest(url, parameters, method);
        }

        private async Task<T> Request<T>(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var json = await OAuthRequest(url, parameters, method);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }

        // All requests return JSON

        private Task<string> OAuthRequest(string url, IEnumerable<(string, string)> parameters, string method)
        {
            if (method != GET && method != POST) throw new ArgumentException($"method parameter must be \"{GET}\" or \"{POST}\"");
            if (ConsumerKey == null) throw new InvalidOperationException("ConsumerKey is null");
            if (ConsumerSecret == null) throw new InvalidOperationException("ConsumerSecret is null");
            if (AccessToken == null) throw new InvalidOperationException("AccessToken is null");
            if (AccessTokenSecret == null) throw new InvalidOperationException("AccessTokenSecret is null");
            return OAuthRequestWorker(url, parameters, method);
        }

        private async Task<string> OAuthRequestWorker(string url, IEnumerable<(string, string)> parameters, string method)
        {
            var post = string.Equals(method, POST, StringComparison.Ordinal);
            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var parray = parameters ?? parameters.ToArray();
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
                using var requestStream = request.GetRequestStream();
                WriteTextToStream(requestStream, string.Join("&", parameterStrings));
            }

            using var response = await request.GetResponseAsync();
            using var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var result = stream.ReadToEnd();
            return result;
        }

        /// <summary>
        /// Twitter requires media upload to be multipart form with specific parameters
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="segmentIndex"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task AppendMedia(string mediaId, int segmentIndex, byte[] payload)
        {
            if (ConsumerKey == null) throw new InvalidOperationException("ConsumerKey is null");
            if (ConsumerSecret == null) throw new InvalidOperationException("ConsumerSecret is null");
            if (AccessToken == null) throw new InvalidOperationException("AccessToken is null");
            if (AccessTokenSecret == null) throw new InvalidOperationException("AccessTokenSecret is null");

            var nonce = OAuth.Nonce();
            var timestamp = OAuth.TimeStamp();
            var uploadUrl = TwitterApi.UploadMediaUrl;
            var signature = OAuth.Signature(POST, uploadUrl, nonce, timestamp, ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret, null);
            var authorizeHeader = OAuth.AuthorizationHeader(nonce, timestamp, ConsumerKey, AccessToken, signature);

            var request = WebRequest.Create(uploadUrl);
            request.Headers.Add("Authorization", authorizeHeader);
            request.Method = POST;

            var boundary = $"{Guid.NewGuid():N}";
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                TextParameter(requestStream, boundary, "command", "APPEND");
                TextParameter(requestStream, boundary, "media_id", mediaId);
                TextParameter(requestStream, boundary, "segment_index", segmentIndex.ToString(CultureInfo.InvariantCulture));
                BinaryParameter(requestStream, boundary, "media", payload);
                WriteTextToStream(requestStream, $"--{boundary}--\r\n");
            }

            using (await request.GetResponseAsync())
            {
                // lint checker likes this better
            }
        }

        private static void TextParameter(Stream stream, string boundary, string name, string payload)
        {
            var header = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n";
            WriteTextToStream(stream, header);
            WriteTextToStream(stream, payload);
            WriteTextToStream(stream, "\r\n");
        }

        private static void BinaryParameter(Stream stream, string boundary, string name, byte[] payload)
        {
            var header =
                $"--{boundary}\r\nContent-Type: application/octet-stream\r\n" +
                $"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n";

            WriteTextToStream(stream, header);
            stream.Write(payload, 0, payload.Length);
            WriteTextToStream(stream, "\r\n");
        }

        private static void WriteTextToStream(Stream stream, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}