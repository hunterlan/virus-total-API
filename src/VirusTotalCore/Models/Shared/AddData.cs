using System.Text.Json.Serialization;

namespace VirusTotalAPI.Models.Shared;


public class AddData<T>
{
    public required string Type { get; set; }
    
    public required T Attributes { get; set; }
}