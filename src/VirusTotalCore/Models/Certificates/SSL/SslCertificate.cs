namespace VirusTotalAPI.Models.Certificates.SSL;

public class SslCertificate
{
    public required CertSignature CertSignature { get; set; }
    public required Extensions Extensions { get; set; }
    public required Issuer Issuer { get; set; }
    public required PublicKey PublicKey { get; set; }
    public required string SerialNumber { get; set; }
    public int Size { get; set; }
    public required Subject Subject { get; set; }
    public required string Thumbprint { get; set; }
    public required string ThumbprintSha256 { get; set; }
    public required Validity Validity { get; set; }
    public required string Version { get; set; }
}