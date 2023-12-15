namespace VirusTotalCore.Models.Comments.IP;

/// <summary>
/// Comments posted in a IP address.
/// </summary>
public class IpComment : BaseComment
{
    public required IpCommentsData[] Data { get; set; }
}