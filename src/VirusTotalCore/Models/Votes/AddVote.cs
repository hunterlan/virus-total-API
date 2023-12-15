using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class AddVote
{
    public required AddData<AddVoteAttribute> Data { get; set; }
}