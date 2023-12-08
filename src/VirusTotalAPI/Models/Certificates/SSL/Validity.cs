namespace VirusTotalAPI.Models.Certificates.SSL;

public class Validity
{
    public DateTimeOffset NotAfter { get; set; }
    public DateTimeOffset NotBefore { get; set; }
}