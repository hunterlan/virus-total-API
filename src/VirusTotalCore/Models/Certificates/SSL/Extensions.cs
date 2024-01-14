namespace VirusTotalCore.Models.Certificates.SSL;

public class Extensions
{
    public List<string> CertificatePolicies { get; set; }
    public List<string> ExtendedKeyUsage { get; set; }
    public AuthorityKeyIdentifier AuthorityKeyIdentifier { get; set; }
    public List<string> SubjectAlternativeName { get; set; }
    public CaInformationAccess CaInformationAccess { get; set; }
    public string SubjectKeyIdentifier { get; set; }
    public List<Uri> CrlDistributionPoints { get; set; }
    public List<string> KeyUsage { get; set; }
    public bool Ca { get; set; }
}