using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace tweetz.core.Services
{
    internal static class TranslateService
    {
        private const string endpoint = "https://api.mymemory.translated.net/get";

        public static async ValueTask<string> Translate(string? text, string fromLanguage, string toLanguage, string? translateApiKey)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "no source text to translate";
            }

            try
            {
                var       parameters     = await BuildParameters(text, fromLanguage, toLanguage, translateApiKey);
                var       requestUri     = new Uri(endpoint + "?" + parameters);
                using var response       = await App.MyHttpClient.GetAsync(requestUri).ConfigureAwait(false);
                var       result         = await response.Content.ReadFromJsonAsync<TranslatorResult>().ConfigureAwait(false);
                var       translatedText = result?.ResponseData?.TranslatedText ?? "no response";
                var       html           = WebUtility.HtmlDecode(WebUtility.HtmlDecode(translatedText)); // Twice to handle sequences like: "&amp;mdash;"
                return html;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static async Task<string> BuildParameters(string? text, string fromLanguage, string toLanguage, string? translateApiKey)
        {
            var pars = new List<KeyValuePair<string?, string?>> {
                new("q", text),
                new("langpair", $"{fromLanguage}|{toLanguage}")
            };

            // The translateApiKey is your email.
            // Add it to your settings file.
            // Use your own real email please.
            // Don't abuse the MyMemoryService.
            // https://mymemory.translated.net/doc/usagelimits.php

            if (!string.IsNullOrWhiteSpace(translateApiKey)) pars.Add(new KeyValuePair<string?, string?>("de", translateApiKey));

            var content    = new FormUrlEncodedContent(pars);
            var parameters = await content.ReadAsStringAsync().ConfigureAwait(false);
            return parameters;
        }
    }

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable ClassNeverInstantiated.Global

    public class TranslatorResult
    {
        public ResponseData? ResponseData { get; set; }
    }

    public class ResponseData
    {
        public string? TranslatedText { get; set; }
    }
}