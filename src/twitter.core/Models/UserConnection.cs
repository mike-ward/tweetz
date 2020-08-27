using System.Linq;
using System.Text.Json.Serialization;

namespace twitter.core.Models
{
    public class UserConnection
    {
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        [JsonPropertyName("connections")]
        public string[] Connections { get; set; }

        public bool IsFollowing { get => Connections?.Contains("following") ?? false; }
        public bool IsFollowedBy { get => Connections?.Contains("followed_by") ?? false; }
    }
}