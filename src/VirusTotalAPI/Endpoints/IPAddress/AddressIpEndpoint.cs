using System.Text.Json;
using RestSharp;
using RestSharp.Serializers.Json;
using VirusTotalAPI.Models;
using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalAPI.Endpoints.IPAddress;

public class AddressIpEndpoint : Endpoint
{
    private JsonSerializerOptions _jsonSerializerOptions = new()
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

        var errorContent = restResponse.Content!;

        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(_jsonSerializerOptions)!;
        ThrowErrorResponseException(errorResponse);
        return new IpAnalysisResult();
    }
}