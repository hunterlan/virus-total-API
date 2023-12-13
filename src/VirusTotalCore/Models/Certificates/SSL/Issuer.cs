namespace VirusTotalCore.Models.Certificates.SSL;

public class Issuer
{
    public required string C { get; set; }
    public required string Cn { get; set; }
    public required string O { get; set; }
}