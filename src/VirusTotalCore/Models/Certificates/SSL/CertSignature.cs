namespace VirusTotalCore.Models.Certificates.SSL;

public class CertSignature
{
    public required string Signature { get; set; }
    public required string SignatureAlgorithm { get; set; }
}