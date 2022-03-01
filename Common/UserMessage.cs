using System.Text.Json.Serialization;

namespace Common
{
    public class UserMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("submittedBy")]
        public string SubmittedBy { get; set; }

        [JsonPropertyName("submittedAt")]
        public DateTimeOffset SubmittedAt { get; set; } = DateTimeOffset.Now;

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}