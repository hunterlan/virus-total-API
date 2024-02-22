
using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class VoteData
{
    public required IEnumerable<Vote> Data { get; set; }
    public required Meta Meta { get; set; }
    public required LinkData Links { get; set; }
}