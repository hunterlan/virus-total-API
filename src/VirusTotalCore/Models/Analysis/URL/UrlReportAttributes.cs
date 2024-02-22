namespace VirusTotalCore.Models.Analysis.URL;

/// <summary>
/// Information about URL.
/// </summary>
public class UrlReportAttributes
{
    public required Dictionary<string, string> Categories { get; set; }
    public int FirstSubmissionDate { get; set; }
    /// <summary>
    /// UTC timestamp representing last time the IP was scanned.
    /// </summary>
    public int LastAnalysisDate { get; set; }
    /// <summary>
    /// Number of different results from this scans.
    /// </summary>
    public required ScanAnalysisStats LastAnalysisStats { get; set; }
    
    /// <summary>
    /// Result from URL scanners. dict with scanner name as key and a dict with notes/result from that scanner as value.
    /// Key presents engine name.
    /// </summary>
    public required Dictionary<string, EngineAnalysisResult> LastAnalysisResults { get; set; }
    public required string LastFinalUrl { get; set; }
    public int LastHttpResponseContentLength { get; set; }
    public required string LastHttpResponseContentSha256 { get; set; }
    public required Dictionary<string, string> LastHttpResponseHeaders { get; set; }
    public required List<string> OutgoingLinks { get; set; }
    public required Dictionary<string, IEnumerable<string>> HtmlMeta { get; set; }
    public int Reputation { get; set; }
    /// <summary>
    /// Identificative attributes.
    /// </summary>
    public required List<string> Tags { get; set; }
    public required List<string> ThreatNames { get; set; }
    /// <summary>
    /// Unweighted number of total votes from the community, divided in "harmless" and "malicious".
    /// </summary>
    public Votes TotalVotes { get; set; } = new();
}