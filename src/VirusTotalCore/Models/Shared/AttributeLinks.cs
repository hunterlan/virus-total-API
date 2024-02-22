namespace VirusTotalCore.Models.Shared;

public class AttributeLinks
{
    /// <summary>
    /// Link to API Comment endpoint to retrieve this commentary by ID later. Can be used in library.
    /// </summary>
    public required Uri Self { get; set; }
}