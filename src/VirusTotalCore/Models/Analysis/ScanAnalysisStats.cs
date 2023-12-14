namespace VirusTotalCore.Models.Analysis;

public class ScanAnalysisStats
{
    public long Harmless { get; set; }
    public long Malicious { get; set; }
    public long Suspicious { get; set; }
    public long Undetected { get; set; }
    public long Timeout { get; set; }
    public long TypeUnsupported { get; set; }
    public long ConfirmedTimeout { get; set; }
    public long Failure { get; set; }
}