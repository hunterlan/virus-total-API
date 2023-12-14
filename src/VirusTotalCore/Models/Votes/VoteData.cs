using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class VoteData
{
    public required Attributes Attributes { get; set; }
    public required AttributeLinks Links { get; set; }
    public required string Type { get; set; }
    public required string Id { get; set; }
}