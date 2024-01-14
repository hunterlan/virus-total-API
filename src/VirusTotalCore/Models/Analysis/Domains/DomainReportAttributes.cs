using VirusTotalCore.Models.Certificates.SSL;

namespace VirusTotalCore.Models.Analysis.Domains;

public class DomainReportAttributes
{
    public required Dictionary<string, string> Categories { get; set; }
    
    public long? CreationDate { get; set; }
    
    /// <summary>
    /// IP address' JARM hash.
    /// </summary>
    public string? Jarm { get; set; }
    
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
    
    /// <summary>
    /// SSL Certificate information for IP.
    /// </summary>
    public SslCertificate? LastHttpsCertificate { get; set; }

    /// <summary>
    /// Date when the certificate shown in <see cref="LastHttpsCertificate"/> was retrieved by VirusTotal. UTC timestamp.
    /// </summary>
    public int? LastHttpsCertificateDate { get; set; }
    
    public string? Registrar { get; set; }
    
    /// <summary>
    /// Identificative attributes.
    /// </summary>
    public List<string>? Tags { get; set; }
    
    /// <summary>
    /// Unweighted number of total votes from the community, divided in "harmless" and "malicious".
    /// </summary>
    public Votes TotalVotes { get; set; } = new();
    
    /// <summary>
    /// Whois information as returned from the pertinent whois server.
    /// </summary>
    public string? WhoIs { get; set; }

    /// <summary>
    /// Date of the last update of the whois record in VirusTotal. UTC timestamp.
    /// </summary>
    public int? WhoIsDate { get; set; }
}