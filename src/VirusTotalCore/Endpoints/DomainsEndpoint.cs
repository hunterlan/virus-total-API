using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Models.Analysis.Domains;
using VirusTotalCore.Models.Analysis.IP;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class DomainsEndpoint(string apiKey) : BaseEndpoint(apiKey, "/domains")
{
    public async Task<DomainAnalysisReport> GetReport(string domain, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{domain}").AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<DomainAnalysisReport>(JsonSerializerOptions)!;
        return result;
    }

    public async Task<Comment> GetComments(string domain, CancellationToken? cancellationToken, int? limit, string? cursor)
    {
        var finalResource = $"/{domain}/comments";
        if (limit is not null)
        {
            finalResource += $"?limit={limit.ToString()}";
            if (cursor is not null)
            {
                finalResource += $"&cursor={cursor}";
            }
        }
        else
        {
            if (cursor is not null)
            {
                finalResource += $"?cursor={cursor}";
            }
        }

        var request = new RestRequest(finalResource).AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    public async Task AddComment(string domain, string comment, CancellationToken? cancellationToken)
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
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
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

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Vote>(JsonSerializerOptions)!;
        return result;
    }

    public async Task AddVote(string domain, VerdictType verdict, CancellationToken? cancellationToken)
    {
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

        var requestUrl = $"/{domain}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}