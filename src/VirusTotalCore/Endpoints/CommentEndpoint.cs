using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Comments.Vote;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

/// <summary>
/// Get comments, delete own comments or add votes to comment.
/// </summary>
/// <param name="apiKey">User's API key</param>
public class CommentEndpoint(string apiKey) : BaseEndpoint(apiKey, "/comments")
{
    /// <summary>
    /// This endpoint retrieves information about the latest comments added to VirusTotal.
    /// </summary>
    /// <param name="filter"> Optional. Do some filtering over those comments, and get only those that contains a certain tag inside
    /// (e.g. filter=tag:malware).</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Number of items to retrieve. Default value is 10.</param>
    /// <returns>Latest comments.</returns>
    public async Task<CommentData> GetLatest(string? filter, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var parameters = new { limit, filter, cursor };

        var request = new RestRequest().AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<CommentData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Get specific comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Specified comment.</returns>
    public async Task<Comment> Get(string commentId, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{commentId}";
        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Delete comment by ID.
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Delete(string commentId, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{commentId}";
        var request = new RestRequest(requestUrl, Method.Delete);
        var restResponse = await DeleteResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    /// <summary>
    /// Add vote to comment
    /// </summary>
    /// <param name="commentId">Comment ID</param>
    /// <param name="verdict">"Positive", "Negative" or "Abuse"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task AddVote(string commentId, CommentVerdict verdict, CancellationToken? cancellationToken)
    {
        var newVote = new { Data = verdict.ToString().ToLower() };

        var requestUrl = $"/{commentId}/vote";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}