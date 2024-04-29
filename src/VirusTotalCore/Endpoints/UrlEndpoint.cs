using VirusTotalCore.Enums;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.URL;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

/// <summary>
/// Analyse URLs, get reports, comments and votes about it and owns.
/// </summary>
/// <param name="apiKey">User's API key.</param>
public class UrlEndpoint : BaseEndpoint
{
    public UrlEndpoint(string apiKey) : base(apiKey, "files/") { }
    public UrlEndpoint(HttpClient customHttpClient, string apiKey) : base(customHttpClient, apiKey, "files/") { }
    // TODO: Rewrite it
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
        using var response = await client.SendAsync(request);
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
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<UrlReportAttributes>>(ToBase64String(url), rootPropertyName, cancellationToken ?? new CancellationToken());
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
        var requestUrl = $"{id}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        return await GetAsync<CommentData>(requestUrl, cancellationToken ?? new CancellationToken());
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
        const string rootPropertyName = "data";
        var newComment = new AddComment(comment);
        var requestUrl = $"{id}/comments";

        return await PostAsync<Comment>(requestUrl, rootPropertyName, newComment, cancellationToken ?? new CancellationToken());
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
        var requestUrl = $"{id}/votes";
        return await GetAsync<VoteData>(requestUrl, cancellationToken ?? new CancellationToken());
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
        var newVote = new AddVote(verdict);
        var requestUrl = $"{id}/votes";
        
        await PostAsync(requestUrl, newVote, cancellationToken ?? new CancellationToken());
    }

    public override async Task<string> GetRelatedObjects(string id, string relationship, string? cursor,
        CancellationToken? cancellationToken, int limit = 10)
    {
        return await base.GetRelatedObjects(id, relationship, cursor, cancellationToken, limit);
    }

    private static string ToBase64String(string plainText) 
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}