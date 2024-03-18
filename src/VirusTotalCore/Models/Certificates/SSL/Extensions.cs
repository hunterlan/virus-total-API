namespace VirusTotalCore.Models.Certificates.SSL;

public class Extensions
{
    public required List<string> CertificatePolicies { get; set; }
    public required List<string> ExtendedKeyUsage { get; set; }
    public required AuthorityKeyIdentifier AuthorityKeyIdentifier { get; set; }
    public required List<string> SubjectAlternativeName { get; set; }
    public required CaInformationAccess CaInformationAccess { get; set; }
    public required string SubjectKeyIdentifier { get; set; }
    public required List<Uri> CrlDistributionPoints { get; set; }
    public required List<string> KeyUsage { get; set; }
    public bool Ca { get; set; }
}