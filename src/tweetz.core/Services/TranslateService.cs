using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tweetz.core.Services
{
    public static class TranslateService
    {
        private const string serviceKey = "c32750406743418d86186b20b9903154";
        public static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";

        public static async ValueTask<string> Translate(string? source, string toLanguage)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "no source text to translate";
            }

            try
            {
                var uri = endpoint + $"&to={toLanguage}";

                var request = WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Ocp-Apim-Subscription-Key", serviceKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", "centralus");
                request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

                var body = new object[] { new { Text = source } };
                var requestBody = JsonSerializer.Serialize(body);
                var bytes = Encoding.UTF8.GetBytes(requestBody);

                var content = await request.GetRequestStreamAsync().ConfigureAwait(false);
                await content.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                content.Close();

                using var response = await request.GetResponseAsync().ConfigureAwait(false);
                using var stream = response.GetResponseStream();
                var result = await JsonSerializer.DeserializeAsync<TranslatorResult[]>(stream).ConfigureAwait(false);
                return result![0].Translations![0].Text!;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0048:File name must match type name")]
    public class TranslatorResult
    {
        [JsonPropertyName("translations")]
        public IList<TranslationsItem>? Translations { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0048:File name must match type name")]
    public class TranslationsItem
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("to")]
        public string? To { get; set; }
    }
}