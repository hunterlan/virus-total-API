using VirusTotalAPI.Models.Shared;
using VirusTotalAPI.Models.Votes;

namespace VirusTotalAPI.Models.Add;

public class AddVote
{
    public required AddData<AddVoteAttribute> Data { get; set; }
}