using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Models;
using VirusTotalAPI.Models.Analysis.IP;
using VirusTotalAPI.Models.Comments.IP;
using VirusTotalAPI.Models.Comments.IP.Add;

namespace VirusTotalAPI.Endpoints;

public class AddressIpEndpoint : Endpoint
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };
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
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<IpAnalysisResult>(_jsonSerializerOptions)!;
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
        var result = resultJsonDocument.Deserialize<IpComment>(_jsonSerializerOptions)!;
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

        var serializedJson = JsonSerializer.Serialize(newComment, _jsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddHeader("x-apikey", ApiKey)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
    }

    //TODO: Get objects related to an IP Address
    
    //TODO: Get objects descriptors related to an IP Address
    
    //TODO: Get votes on an IP address
    
    //TODO: Post vote to IP Address

    private Exception HandleError(string errorContent)
    {
        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(_jsonSerializerOptions)!;
        return ThrowErrorResponseException(errorResponse);
    }

    private Task<RestResponse> GetResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null 
            ? Client.ExecuteGetAsync(request, cancellationToken.Value) 
            : Client.ExecuteGetAsync(request);
    }

    private Task<RestResponse> PostResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null
            ? Client.ExecutePostAsync(request, cancellationToken.Value)
            : Client.ExecutePostAsync(request);
    }
}