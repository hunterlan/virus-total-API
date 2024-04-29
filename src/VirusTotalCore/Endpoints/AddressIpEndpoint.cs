using VirusTotalCore.Enums;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.IP;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class AddressIpEndpoint : BaseEndpoint
{
    public AddressIpEndpoint(string apiKey) : base(apiKey, "files/") { }
    public AddressIpEndpoint(HttpClient customHttpClient, string apiKey) : base(customHttpClient, apiKey, "files/") { }
    /// <summary>
    ///     Get report on given ip address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="AddressReportAttributes">Analysis report</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<AnalysisReport<AddressReportAttributes>> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<AddressReportAttributes>>(ipAddress, rootPropertyName, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    ///     Get comments for given IP address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cursor"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="limit">Maximum number of comments to retrieve. Default is 10.</param>
    /// <returns cref="IpComment">Comments</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    /// <exception cref="AuthenticationRequiredError">Empty API key</exception>
    /// <exception cref="WrongCredentialsError">Invalid API key</exception>
    public async Task<CommentData> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var requestUrl = $"{ipAddress}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    ///     Post your comment to IP address.
    ///     Any word starting with # in your comment's text will be considered a tag,
    ///     and added to the comment's tag attribute.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="comment">Your comment for given IP address</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    /// <exception cref="AlreadyExistsException">Comment with given content is already exists.</exception>
    public async Task AddComment(string ipAddress, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment(comment);
        var requestUrl = $"{ipAddress}/comments";

        await PostAsync(requestUrl, newComment, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    ///     Get community votes for given IP address.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="VoteData">IP address community votes</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<VoteData> GetVotes(string ipAddress, CancellationToken? cancellationToken)
    {
        var requestUrl = $"{ipAddress}/votes";
        return await GetAsync<VoteData>(requestUrl, cancellationToken ?? new CancellationToken());
    }

    /// <summary>
    ///     Post your vote to IP Address.
    ///     The verdict attribute must have be either harmless or malicious.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param cref="VerdictType" name="verdict">Harmless or malicious</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentOutOfRangeException">Only harmless or malicious verdict is available.</exception>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task AddVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote(verdict);
        var requestUrl = $"{ipAddress}/votes";
        
        await PostAsync(requestUrl, newVote, cancellationToken ?? new CancellationToken());
    }
}