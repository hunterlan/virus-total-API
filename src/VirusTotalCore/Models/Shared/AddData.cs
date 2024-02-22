namespace VirusTotalCore.Models.Shared;

/// <summary>
/// Class is used for posting vote or comment
/// </summary>
/// <typeparam name="T">Use AddCommentAttribute or AddVoteAttribute classes</typeparam>
public class AddData<T>
{
    /// <summary>
    /// Post type. Two options: "vote" and "comment"
    /// </summary>
    public required string Type { get; set; }
    
    public required T Attributes { get; set; }
}