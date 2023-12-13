using System.Text.Json.Serialization;

namespace VirusTotalAPI.Models.Analysis.IP;

public class IpAnalysisResult : AnalysisResult
{
    //[JsonPropertyName("attributes")]
    public IpAddressAttributes Attributes { get; set; }
}