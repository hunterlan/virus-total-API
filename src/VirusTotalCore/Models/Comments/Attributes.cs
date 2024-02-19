using VirusTotalCore.Models.Comments.Vote;

namespace VirusTotalCore.Models.Comments;

public class Attributes
{
    /// <summary>
    /// Publication date. UTC Timestamp.
    /// </summary>
    public long Date { get; set; }
    /// <summary>
    /// Comment's content.
    /// </summary>
    public required string Text { get; set; }
    public required CommentVotes Votes { get; set; }
    /// <summary>
    /// HTML version of comment.
    /// </summary>
    public required string Html { get; set; }
    public required string[] Tags { get; set; }
}