using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Comments.Vote;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class CommentEndpoint(string apiKey) : BaseEndpoint(apiKey, "/comments")
{
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

    public async Task Delete(string commentId, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{commentId}";
        var request = new RestRequest(requestUrl, Method.Delete);
        var restResponse = await DeleteResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    public async Task AddVote(string commentId, CommentVerdict verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote<string>
        {
            Data = verdict.ToString().ToLower()
        };

        var requestUrl = $"/{commentId}/vote";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}