using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Models.Analysis.IP;
using VirusTotalAPI.Models.Comments.IP;
using VirusTotalAPI.Models.Comments.IP.Add;
using VirusTotalAPI.Models.Votes;

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
            Data = new Data
            {
                Attributes = new AddCommentAttributes
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

    //TODO: Post vote to IP Address
    public void PostVote(string ipAddress)
    {
        throw new NotImplementedException();
    }
}