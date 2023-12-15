using System.Security.Cryptography;

namespace VirusTotalCore.Models.Certificates.SSL;

public class PublicKey
{
    /// <summary>
    /// Any of "RSA", "DSA" or "EC".
    /// Indicates the algorithm used to generate the certificate.
    /// Depending on this field, one of the following fields is added.
    /// </summary>
    public required string Algorithm { get; set; }
    public Rsa? Rsa { get; set; }
    public DSA? Dsa { get; set; }
    public ECDiffieHellmanCng? Ec { get; set; }
}