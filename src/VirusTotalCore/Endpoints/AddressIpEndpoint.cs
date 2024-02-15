using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models.Analysis.IP;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class AddressIpEndpoint(string apiKey) : BaseEndpoint(apiKey, "/ip_addresses")
{
    /// <summary>
    ///     Get report on given ip address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="AddressAnalysisReport">Analysis report</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<AddressAnalysisReport> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{ipAddress}");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AddressAnalysisReport>(JsonSerializerOptions)!;
        return result;
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
    public async Task<Comment> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var parameters = new { limit, cursor };
        var requestUrl = $"/{ipAddress}/comments";

        var request = new RestRequest(requestUrl).AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
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
    public async Task PostComment(string ipAddress, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment
        {
            Data = new AddData<AddCommentAttribute>
            {
                Type = "comment",
                Attributes = new AddCommentAttribute
                {
                    Text = comment
                }
            }
        };
        var requestUrl = $"/{ipAddress}/comments";

        var serializedJson = JsonSerializer.Serialize(newComment, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    //TODO: Get objects related to an IP Address
    public void GetRelatedObjects(string ipAddress)
    {
        throw new NotImplementedException();
    }

    //TODO: Get objects descriptors related to an IP Address
    public void GetRelatedDescriptors(string ipAddress)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Get community votes for given IP address.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="Vote">IP address community votes</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<Vote> GetVotes(string ipAddress, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{ipAddress}/votes";

        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Vote>(JsonSerializerOptions)!;
        return result;
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
    public async Task PostVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken)
    {
        //TODO: Rewrite Enum to static class
        var userVerdict = verdict switch
        {
            VerdictType.Harmless => "harmless",
            VerdictType.Malicious => "malicious",
            _ => throw new ArgumentOutOfRangeException(nameof(verdict), verdict,
                "The verdict attribute must have be either harmless or malicious.")
        };

        var newVote = new AddVote
        {
            Data = new AddData<AddVoteAttribute>
            {
                Type = "vote",
                Attributes = new AddVoteAttribute
                {
                    Verdict = userVerdict
                }
            }
        };

        var requestUrl = $"/{ipAddress}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}