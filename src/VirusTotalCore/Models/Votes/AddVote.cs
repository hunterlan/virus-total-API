using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Models.Votes;

public class AddVote<T>
{
    public required T Data { get; set; }
}