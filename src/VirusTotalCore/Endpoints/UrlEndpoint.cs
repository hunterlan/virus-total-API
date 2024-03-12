using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.URL;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

/// <summary>
/// Analyse URLs, get reports, comments and votes about it and owns.
/// </summary>
/// <param name="apiKey">User's API key.</param>
public class UrlEndpoint(string apiKey) : BaseEndpoint(apiKey, "/urls")
{
    /// <summary>
    /// Request to scan URL. 
    /// </summary>
    /// <param name="url">URL to scan</param>
    /// <param name="cancellationToken">Cancellation token</param>
    private async Task Scan(string url, CancellationToken? cancellationToken)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/urls");
        request.Headers.Add("x-apikey", ApiKey);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(url), "url");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Get report about url
    /// </summary>
    /// <param name="url">URL to scan</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns cref="UrlReportAttributes">Report analysis</returns>
    /// <exception cref="Exception"></exception>
    public async Task<AnalysisReport<UrlReportAttributes>> GetReport(string url, CancellationToken? cancellationToken)
    {
        await Scan(url, cancellationToken);
        var request = new RestRequest($"/{ToBase64String(url)}");
        var restResponse = await GetResponse(request, cancellationToken);
        
        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AnalysisReport<UrlReportAttributes>>(JsonSerializerOptions)!;
        return result;
    }

    public void Rescan(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get comments about URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of comments to retrieve. By default is 10.</param>
    /// <returns>List of comments with metadata.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<CommentData> GetComments(string id, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var parameters = new { limit, cursor };
        
        var request = new RestRequest($"/{id}/comments").AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<CommentData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Add user's comment to URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="comment">Comment content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comment data</returns>
    /// <exception cref="Exception"></exception>
    public async Task<Comment> AddComment(string id, string comment, CancellationToken? cancellationToken)
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
        
        var serializedJson = JsonSerializer.Serialize(newComment, JsonSerializerOptions);
        var request = new RestRequest($"/{id}/comments")
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
        
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// Get votes on URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Votes with metadata</returns>
    /// <exception cref="Exception"></exception>
    public async Task<VoteData> GetVotes(string id, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{id}/votes";

        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<VoteData>(JsonSerializerOptions)!;
        return result;
    }

    
    /// <summary>
    /// Add user's vote to URL.
    /// </summary>
    /// <param name="id">URL identifier</param>
    /// <param name="verdict">"Harmless" or "Malicious"</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="Exception"></exception>
    public async Task AddVote(string id, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote
        {
            Data = new AddData<AddVoteAttribute>
            {
                Type = "vote",
                Attributes = new AddVoteAttribute
                {
                    Verdict = verdict.ToString().ToLower()
                }
            }
        };

        var requestUrl = $"/{id}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    private static string ToBase64String(string plainText) 
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}