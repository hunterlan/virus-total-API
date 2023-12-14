using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Comments;

public class AddComment
{
    public required AddData<AddCommentAttribute> Data { get; set; }
}