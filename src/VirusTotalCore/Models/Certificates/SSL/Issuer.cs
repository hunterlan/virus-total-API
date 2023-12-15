namespace VirusTotalCore.Models.Certificates.SSL;

/// <summary>
/// Certificate's issuer data.
/// </summary>
public class Issuer
{
    /// <summary>
    /// CountryName
    /// </summary>
    public required string C { get; set; }
    /// <summary>
    /// CommonName.
    /// </summary>
    public required string Cn { get; set; }
    /// <summary>
    /// Locality.
    /// </summary>
    public string? L { get; set; }
    /// <summary>
    /// Organization.
    /// </summary>
    public required string O { get; set; }
    /// <summary>
    /// OrganizationalUnit.
    /// </summary>
    public string? Ou { get; set; }
    /// <summary>
    /// StateOrProvinceName.
    /// </summary>
    public string? St { get; set; }
}