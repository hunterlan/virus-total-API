namespace VirusTotalAPI.Models.Comments.IP.Add;

public class Data
{
    public string Type { get; set; } = "comment";
    public required AddCommentAttributes Attributes { get; set; }
}