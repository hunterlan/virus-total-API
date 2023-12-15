namespace VirusTotalCore.Models.Analysis;

public class Votes
{
    /// <summary>
    /// Number of positive votes.
    /// </summary>
    public int Harmless { get; set; }

    /// <summary>
    /// Number of negative votes.
    /// </summary>
    public int Malicious { get; set; }
}