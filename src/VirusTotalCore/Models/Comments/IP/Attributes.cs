namespace VirusTotalAPI.Models.Comments.IP;

public class Attributes
{
    public long Date { get; set; }
    public required string Text { get; set; }
    public required Votes Votes { get; set; }
    public required string Html { get; set; }
    public required string[] Tags { get; set; }
}