namespace VirusTotalAPI.Models.Votes;

public class Attributes
{
    public long Date { get; set; }
    public required string Verdict { get; set; }
    public long Value { get; set; }
}