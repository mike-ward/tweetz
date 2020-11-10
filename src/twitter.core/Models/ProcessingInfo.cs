using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class ProcessingInfo
    {
        public static readonly ProcessingInfo Empty = new();

        public const string StateSucceeded = "succeeded";
        public const string StateInProgress = "in_progress";
        public const string StateFailed = "failed";

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("check_after_secs")]
        public double CheckAfterSecs { get; set; }

        [JsonPropertyName("progress_percent")]
        public double ProgressPercent { get; set; }

        [JsonPropertyName("error")]
        public ProcessingError Error { get; set; } = ProcessingError.Empty;
    }
}