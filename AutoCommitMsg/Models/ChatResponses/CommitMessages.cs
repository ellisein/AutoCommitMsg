using System.Text.Json.Serialization;

namespace AutoCommitMsg.Models.ChatResponses;

public class CommitMessages
{
    [JsonPropertyName("Messages")]
    public List<string> Messages { get; set; } = [];
}
