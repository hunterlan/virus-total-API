namespace VirusTotalCore.Models.Certificates.SSL;
/// <summary href="https://docs.virustotal.com/reference/ssl-certificate">
/// SSL certificates associated with domains and IPs.
/// </summary>
public class SslCertificate
{
    public required CertSignature CertSignature { get; set; }
    /// <summary>
    /// Dictionary containing all certificate's extensions. Subfields may vary. More details
    /// <see href="https://docs.virustotal.com/reference/ssl-certificate">here.</see>
    /// </summary>
    public required Dictionary<string, string> Extensions { get; set; }
    /// <summary>
    /// Containing the certificate's issuer data.
    /// </summary>
    public required Issuer Issuer { get; set; }
    /// <summary>
    /// Public key info
    /// </summary>
    public required PublicKey PublicKey { get; set; }
    /// <summary>
    /// Certificate's serial number hexdump.
    /// </summary>
    public required string SerialNumber { get; set; }
    /// <summary>
    /// Certificate content length.
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// Containing the certificate's subject data.
    /// </summary>
    //TODO: It has same field with Issuer
    public required Subject Subject { get; set; }
    /// <summary>
    /// Certificate's content SHA1 hash.
    /// </summary>
    public required string Thumbprint { get; set; }
    /// <summary>
    /// Certificate's content SHA256 hash.
    /// </summary>
    public required string ThumbprintSha256 { get; set; }
    /// <summary>
    /// Defines certificate's validity period
    /// </summary>
    public required Validity Validity { get; set; }
    /// <summary>
    /// Certificate version (typically "V1", "V2" or "V3").
    /// </summary>
    public required string Version { get; set; }
}