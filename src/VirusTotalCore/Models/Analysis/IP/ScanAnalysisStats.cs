namespace VirusTotalAPI.Models.Analysis.IP;

public class ScanAnalysisStats
{
    public long Harmless { get; set; }
    public long Malicious { get; set; }
    public long Suspicious { get; set; }
    public long Undetected { get; set; }
    public long Timeout { get; set; }
}