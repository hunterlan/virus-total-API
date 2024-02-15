using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Comments;

namespace VirusTotalCore.Endpoints;

public class CommentEndpoint(string apiKey) : BaseEndpoint(apiKey, "/comments")
{
    public async Task<Comment> GetLatest(string? filter, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var parameters = new { limit, filter, cursor };

        var request = new RestRequest().AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    public void GetObject()
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public void AddVote()
    {
        throw new NotImplementedException();
    }
}