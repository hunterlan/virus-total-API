namespace VirusTotalCore.Models.Certificates.SSL;

public class Rsa
{
    public required string Exponent { get; set; }
    public long KeySize { get; set; }
    public required string Modulus { get; set; }
}