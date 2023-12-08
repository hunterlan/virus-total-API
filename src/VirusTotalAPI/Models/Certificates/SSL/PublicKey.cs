namespace VirusTotalAPI.Models.Certificates.SSL;

public class PublicKey
{
    public string Algorithm { get; set; }
    public Rsa Rsa { get; set; }
}