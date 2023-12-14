using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Models.Add;

public class AddVote
{
    public required AddData<AddVoteAttribute> Data { get; set; }
}