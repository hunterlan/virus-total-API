namespace VirusTotalCore.Models.Certificates.SSL;

public class CertSignature
{
    /// <summary>
    /// Signature hexdump.
    /// </summary>
    public required string Signature { get; set; }
    /// <summary>
    /// Used algorithm (i.e. "sha256RSA").
    /// </summary>
    public required string SignatureAlgorithm { get; set; }
}