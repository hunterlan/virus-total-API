using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class VoteData
{
    /// <summary>
    /// Data about a specific vote.
    /// </summary>
    public required Attributes Attributes { get; set; }
    /// <summary>
    /// Contains "self", with a link to the vote itself.
    /// </summary>
    public required AttributeLinks Links { get; set; }
    /// <summary>
    /// Resource identifier of the vote.
    /// </summary>
    public required string Id { get; set; }
}