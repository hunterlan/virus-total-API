namespace VirusTotalAPI.Models.Certificates.SSL;

public class Extensions
{
    public string The13614111129242 { get; set; }
    public bool Ca { get; set; }
    public AuthorityKeyIdentifier AuthorityKeyIdentifier { get; set; }
    public CaInformationAccess CaInformationAccess { get; set; }
    public List<string> CertificatePolicies { get; set; }
    public List<string> ExtendedKeyUsage { get; set; }
    public List<string> KeyUsage { get; set; }
    public List<string> SubjectAlternativeName { get; set; }
    public string SubjectKeyIdentifier { get; set; }
}