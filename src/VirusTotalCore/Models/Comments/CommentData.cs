using System.Text.Json.Serialization;
using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class CommentData
{
    [JsonPropertyName("Data")]
    public required Comment[] Comments { get; set; }
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}