using VirusTotalAPI.Models.Shared;

namespace VirusTotalAPI.Models.Comments;

public class BaseComment
{
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}