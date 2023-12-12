using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Models;
using VirusTotalAPI.Models.Analysis.IP;
using VirusTotalAPI.Models.Comments.IP;

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

        RestResponse restResponse;

        if (cancellationToken is not null)
        {
            restResponse = await Client.ExecuteGetAsync(request, cancellationToken.Value);
        }
        else
        {
            restResponse = await Client.ExecuteGetAsync(request);
        }

        if (restResponse is { IsSuccessful: true})
        {
            var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
            var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<IpAnalysisResult>(_jsonSerializerOptions)!;
            return result;
        }

        HandleError(restResponse.Content!);
        return new IpAnalysisResult();
    }

    public async Task<IpComment> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken, int limits = 10)
    {
        var requestUrl = $"/{ipAddress}/comments?limit={limits}";

        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        var request = new RestRequest(requestUrl).AddHeader("x-apikey", ApiKey);
        
        RestResponse restResponse;

        if (cancellationToken is not null)
        {
            restResponse = await Client.ExecuteGetAsync(request, cancellationToken.Value);
        }
        else
        {
            restResponse = await Client.ExecuteGetAsync(request);
        }
        
        if (restResponse is { IsSuccessful: true})
        {
            var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
            var result = resultJsonDocument.Deserialize<IpComment>(_jsonSerializerOptions)!;
            return result;
        }

        HandleError(restResponse.Content!);
        return new IpComment();
    }

    private void HandleError(string errorContent)
    {
        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(_jsonSerializerOptions)!;
        ThrowErrorResponseException(errorResponse);
    }
}