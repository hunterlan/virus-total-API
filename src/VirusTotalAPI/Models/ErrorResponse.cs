using System.Text.Json.Serialization;

namespace VirusTotalAPI.Models;

public class ErrorResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
    [JsonPropertyName("code")]
    public required string Code { get; set; }
}