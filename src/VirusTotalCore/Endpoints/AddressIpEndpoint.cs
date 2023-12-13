using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Enums;
using VirusTotalAPI.Models.Add;
using VirusTotalAPI.Models.Analysis.IP;
using VirusTotalAPI.Models.Comments;
using VirusTotalAPI.Models.Comments.IP;
using VirusTotalAPI.Models.Shared;
using VirusTotalAPI.Models.Votes;
using VirusTotalCore;
using VirusTotalCore.Models.Analysis.IP;

namespace VirusTotalAPI.Endpoints;

public class AddressIpEndpoint : Endpoint
{
    public AddressIpEndpoint(string apiKey)
    {
        Url += "/ip_addresses";
        ApiKey = apiKey;
        var options = new RestClientOptions(Url);
        Client = new RestClient(options);
    }

    public async Task<IpAnalysisResult> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{ipAddress}").AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<IpAnalysisResult>(JsonSerializerOptions)!;
        return result;
    }

    public async Task<IpComment> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken, int limits = 10)
    {
        var requestUrl = $"/{ipAddress}/comments?limit={limits}";

        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<IpComment>(JsonSerializerOptions)!;
        return result;
    }

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
            .AddHeader("x-apikey", ApiKey)
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

    public async Task<Vote> GetVotes(string ipAddress, CancellationToken? cancellationToken)
    {
        var requestUrl = $"/{ipAddress}/votes";

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<Vote>(JsonSerializerOptions)!;
        return result;
    }
    
    public async Task PostVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken)
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

        var requestUrl = $"/{ipAddress}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }
}