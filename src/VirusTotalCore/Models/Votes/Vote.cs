
using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class Vote
{
    public required VoteData[] Data { get; set; }
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}