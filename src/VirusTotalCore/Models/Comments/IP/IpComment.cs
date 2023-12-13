namespace VirusTotalAPI.Models.Comments.IP;

public class IpComment : BaseComment
{
    public required IpCommentsData[] Data { get; set; }
}