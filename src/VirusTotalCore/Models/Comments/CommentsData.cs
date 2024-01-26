using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class CommentsData
{
    public required Attributes Attributes { get; set; }
    public required string Id { get; set; }
    public required AttributeLinks Links { get; set; }
}