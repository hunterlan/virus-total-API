using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.Domains;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class DomainsEndpoint(string apiKey) : BaseEndpoint(apiKey, "/domains")
{
    /// <summary>
    /// Get report about specific domain
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancelattion token</param>
    /// <returns cref="DomainReportAttributes">Analysis report</returns>
    public async Task<AnalysisReport<DomainReportAttributes>> GetReport(string domain, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{domain}");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AnalysisReport<DomainReportAttributes>>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Get comments about domain.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    /// <returns>List of comments with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<CommentData> GetComments(string domain, CancellationToken? cancellationToken, string? cursor, 
        int limit = 10)
    {
        var finalResource = $"/{domain}/comments";
        var parameters = new { limit, cursor };

        var request = new RestRequest(finalResource).AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<CommentData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Add new comment about domain.
    /// Any word starting with # in your comment's text will be considered a tag,
    /// and added to the comment's tag attribute.
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="comment">Comment content.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comment data</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Comment> AddComment(string domain, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment(comment);
        var requestUrl = $"/{domain}/comments";

        var serializedJson = JsonSerializer.Serialize(newComment, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
        
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    public void GetDnsResolution()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get votes data on domain
    /// </summary>
    /// <param name="domain">Domain name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Votes with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<VoteData> GetVotes(string domain, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{domain}/votes";

        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<VoteData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Add user's vote to specific domain.
    /// </summary>
    /// <param name="domain">Domain name.</param>
    /// <param name="verdict">"Harmless" or "Malicious"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="Exception"></exception>
    public async Task AddVote(string domain, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote(verdict);

        var requestUrl = $"/{domain}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}