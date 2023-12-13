using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalCore.Models.Analysis.IP;

public class IpAnalysisResult : AnalysisResult
{
    public required IpAddressAttributes Attributes { get; set; }
}