using VirusTotalCore.Models.Certificates.SSL;

namespace VirusTotalCore.Models.Analysis.IP;

/// <summary href="https://docs.virustotal.com/reference/ip-object">
/// IPv4 addresses are other of the network locations that VirusTotal stores information about.
/// </summary>
public class AddressReportAttributes
{
    /// <summary>
    /// IPv4 network range to which the IP belongs.
    /// </summary>
    public string Network { get; set; } = "";

    /// <summary>
    /// Identificative attributes.
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// Country where the IP is placed (ISO-3166 country code).
    /// </summary>
    public string Country { get; set; } = "";

    /// <summary>
    /// Continent where the IP is placed (ISO-3166 continent code).
    /// </summary>
    public string Continent { get; set; } = "";

    /// <summary>
    /// Owner of the Autonomous System to which the IP belongs.
    /// </summary>
    public string AsOwner { get; set; } = "";

    /// <summary>
    /// Autonomous System Number to which the IP belongs.
    /// </summary>
    public int Asn { get; set; }

    /// <summary>
    /// IP's score calculated from the votes of the VirusTotal's community.
    /// </summary>
    public int Reputation { get; set; }

    /// <summary>
    /// IP address' JARM hash.
    /// </summary>
    public string? Jarm { get; set; }

    /// <summary>
    /// Unweighted number of total votes from the community, divided in "harmless" and "malicious".
    /// </summary>
    public Votes TotalVotes { get; set; } = new();

    /// <summary>
    /// UTC timestamp representing last time the IP was scanned.
    /// </summary>
    public int LastAnalysisDate { get; set; }

    /// <summary>
    /// Date when any of the IP's information was last updated. UTC timestamp.
    /// </summary>
    public int LastModificationDate { get; set; }

    /// <summary>
    /// Number of different results from this scans.
    /// </summary>
    public required ScanAnalysisStats LastAnalysisStats { get; set; }

    /// <summary>
    /// SSL Certificate information for IP.
    /// </summary>
    public SslCertificate? LastHttpsCertificate { get; set; }

    /// <summary>
    /// Date when the certificate shown in <see cref="LastHttpsCertificate"/> was retrieved by VirusTotal. UTC timestamp.
    /// </summary>
    public int? LastHttpsCertificateDate { get; set; }

    /// <summary>
    /// Result from URL scanners. dict with scanner name as key and a dict with notes/result from that scanner as value.
    /// Key presents engine name.
    /// </summary>
    public required Dictionary<string, EngineAnalysisResult> LastAnalysisResults { get; set; }

    /// <summary>
    /// RIR (one of the current RIRs: AFRINIC, ARIN, APNIC, LACNIC or RIPE NCC).
    /// </summary>
    public required string RegionalInternetRegistry { get; set; }

    /// <summary>
    /// Whois information as returned from the pertinent whois server.
    /// </summary>
    public string? WhoIs { get; set; }

    /// <summary>
    /// Date of the last update of the whois record in VirusTotal. UTC timestamp.
    /// </summary>
    public int? WhoIsDate { get; set; }
}