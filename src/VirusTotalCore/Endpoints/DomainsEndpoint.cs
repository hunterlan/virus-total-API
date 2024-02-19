using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Models.Analysis.Domains;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class DomainsEndpoint(string apiKey) : BaseEndpoint(apiKey, "/domains")
{
    public async Task<DomainAnalysisReport> GetReport(string domain, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{domain}");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<DomainAnalysisReport>(JsonSerializerOptions)!;
        return result;
    }

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

    public async Task<Comment> AddComment(string domain, string comment, CancellationToken? cancellationToken)
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

    public void GetObjectsRelated()
    {
        throw new NotImplementedException();
    }

    public void GetObjectDescriptors()
    {
        throw new NotImplementedException();
    }

    public void GetDnsResolution()
    {
        throw new NotImplementedException();
    }

    public async Task<Vote> GetVotes(string domain, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{domain}/votes";

        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Vote>(JsonSerializerOptions)!;
        return result;
    }

    public async Task AddVote(string domain, VerdictType verdict, CancellationToken? cancellationToken)
    {
        var newVote = new AddVote<AddData<AddVoteAttribute>>
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

        var requestUrl = $"/{domain}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}