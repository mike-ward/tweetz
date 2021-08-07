using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tweetz.core.Services
{
    internal static class TranslateService
    {
        private const string endpoint = "https://libretranslate.com/translate";

        public static async ValueTask<string> Translate(string? text, string fromLanguage, string toLanguage, string translateApiKey)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "no source text to translate";
            }

            try
            {
                var request = new HttpRequestMessage {
                    Method     = HttpMethod.Post,
                    RequestUri = new Uri(endpoint),
                    Content = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string?, string?>("q", text),
                        new KeyValuePair<string?, string?>("source", fromLanguage),
                        new KeyValuePair<string?, string?>("target", toLanguage),
                        new KeyValuePair<string?, string?>("api_key", translateApiKey)
                    })
                };

                using var response = await App.MyHttpClient.SendAsync(request).ConfigureAwait(false);
                var       result   = await response.Content.ReadFromJsonAsync<TranslatorResult>().ConfigureAwait(false);
                return result?.TranslatedText ?? result?.ErrorText ?? "no response";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class TranslatorResult
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [JsonPropertyName("translatedText")] public string? TranslatedText { get; set; }
        [JsonPropertyName("error")]          public string? ErrorText      { get; set; }
    }
}