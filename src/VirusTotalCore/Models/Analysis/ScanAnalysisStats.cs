namespace VirusTotalCore.Models.Analysis;

public class ScanAnalysisStats
{
    // Number of reports saying that is harmless.
    public long Harmless { get; set; }
    // Number of reports saying that is malicious.
    public long Malicious { get; set; }
    // Number of reports saying that is suspicious.
    public long Suspicious { get; set; }
    // Number of reports saying that is undetected.
    public long Undetected { get; set; }
    // Number of timeouts when checking this URL.
    public long Timeout { get; set; }
    public long TypeUnsupported { get; set; }
    public long ConfirmedTimeout { get; set; }
    public long Failure { get; set; }
}