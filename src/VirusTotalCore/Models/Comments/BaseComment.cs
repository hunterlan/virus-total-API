using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class BaseComment
{
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}