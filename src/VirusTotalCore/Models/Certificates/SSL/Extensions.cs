namespace VirusTotalCore.Models.Certificates.SSL;

public class Extensions
{
    //TODO: Look at the documentation about that
    //public string The13614111129242 { get; set; }
    public required bool Ca { get; set; }
    public required AuthorityKeyIdentifier AuthorityKeyIdentifier { get; set; }
    public required CaInformationAccess CaInformationAccess { get; set; }
    public required List<string> CertificatePolicies { get; set; }
    public required List<string> ExtendedKeyUsage { get; set; }
    public required List<string> KeyUsage { get; set; }
    public required List<string> SubjectAlternativeName { get; set; }
    public required string SubjectKeyIdentifier { get; set; }
}