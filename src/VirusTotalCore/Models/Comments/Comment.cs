using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class Comment
{
    public required CommentsData[] Data { get; set; }
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}