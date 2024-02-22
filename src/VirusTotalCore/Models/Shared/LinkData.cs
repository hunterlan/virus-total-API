namespace VirusTotalCore.Models.Shared;

/// <summary>
/// Contains link to itself. Also might contain a link to next N comments or votes
/// </summary>
public class LinkData
{
    public Uri? Next { get; set; }
    // A link to the vote/comment itself.
    public required Uri Self { get; set; }
}