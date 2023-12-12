namespace VirusTotalAPI.Models.Comments.IP;

public class Attributes
{
    public long Date { get; set; }
    public string Text { get; set; }
    public Votes Votes { get; set; }
    public string Html { get; set; }
    public string[] Tags { get; set; }
}