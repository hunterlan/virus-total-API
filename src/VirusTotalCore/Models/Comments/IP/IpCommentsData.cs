using VirusTotalAPI.Models.Shared;

namespace VirusTotalAPI.Models.Comments.IP;

public class IpCommentsData
{
    public required Attributes Attributes { get; set; }
    public required string Type { get; set; }
    public required string Id { get; set; }
    public required AttributeLinks Links { get; set; }
}