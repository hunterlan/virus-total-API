using System.Security.Cryptography;
using VirusTotalAPI.Models.Certificates.SSL;
using VirusTotalAPI.Models.IP;

namespace VirusTotalAPI.Models.Analysis.IP;

public class IpAddressAttributes
{
    public int? FirstSubmissionDate { get; set; }
    
    /// <summary>
    /// Difference hash and md5 hash of the URL's favicon. Only returned in premium API.
    /// </summary>
    public Favicon? Favicon { get; set; }
 
    public Dictionary<string, string>? Categories { get; set; }
    
    public Dictionary<string, List<string>>? HtmlMeta { get; set; }

    public string Network { get; set; } = "";
    
    public List<string>? Tags { get; set; }

    public string Country { get; set; } = "";

    public string Continent { get; set; } = "";

    public string AsOwner { get; set; } = "";
    
    public int Asn { get; set; }
    
    public int Reputation { get; set; }
    
    public string? Jarm { get; set; }

    public Votes TotalVotes { get; set; } = new();
    
    public int LastAnalysisDate { get; set; }
    
    public int LastModificationDate { get; set; }

    public required ScanAnalysisStats LastAnalysisStats { get; set; }
    
    public SslCertificate? LastHttpsCertificate { get; set; }
    
    public int? LastHttpsCertificateDate { get; set; }
    
    //TODO: Here they are played themselves, because keys starts from Uppercase, what is problem for me
    public Dictionary<string, EngineAnalysisResult> LastAnalysisResult { get; set; }
    
    public required string RegionalInternetRegistry { get; set; }
    
    public string? WhoIs { get; set; }
    
    public int? WhoIsDate { get; set; }
}