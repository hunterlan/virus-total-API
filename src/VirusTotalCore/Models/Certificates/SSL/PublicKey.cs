namespace VirusTotalCore.Models.Certificates.SSL;

public class PublicKey
{
    public required string Algorithm { get; set; }
    public required Rsa Rsa { get; set; }
}