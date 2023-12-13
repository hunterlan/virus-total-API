namespace VirusTotalAPI.Models.Certificates.SSL;

public class Validity
{
    public required string NotAfter { get; set; }
    public required string NotBefore { get; set; }
}