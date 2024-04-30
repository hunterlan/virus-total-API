using System.Diagnostics.CodeAnalysis;
using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class AddComment
{
    public required AddData<AddCommentAttribute> Data { get; set; }

    [SetsRequiredMembers]
    public AddComment(string comment)
    {
        Data = new AddData<AddCommentAttribute>
        {
            Type = "comment",
            Attributes = new AddCommentAttribute
            {
                Text = comment
            }
        };
    }
}