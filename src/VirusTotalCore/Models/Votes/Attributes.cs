namespace VirusTotalCore.Models.Votes;

public class Attributes
{
    /// <summary>
    /// When the vote was done.
    /// </summary>
    public long Date { get; set; }
    /// <summary>
    /// Note if vote was for making it "malicious" or "harmless".
    /// </summary>
    public required string Verdict { get; set; }
    /// <summary>
    /// Weight given by this vote (positive or negative) for Community Score.
    /// </summary>
    public long Value { get; set; }
}