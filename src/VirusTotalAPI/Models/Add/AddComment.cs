using VirusTotalAPI.Models.Comments;
using VirusTotalAPI.Models.Shared;

namespace VirusTotalAPI.Models.Add;

public class AddComment
{
    public required AddData<AddCommentAttribute> Data { get; set; }
}