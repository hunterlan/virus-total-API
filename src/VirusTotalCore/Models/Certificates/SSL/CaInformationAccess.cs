namespace VirusTotalCore.Models.Certificates.SSL;

public class CaInformationAccess
{
    public Uri? CaIssuers { get; set; }
    public required Uri Ocsp { get; set; }
}