using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Models;
using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalAPI.Endpoints.IPAddress;

public class AdressIpEndpoint : Endpoint
{
    public AdressIpEndpoint(string apiKey)
    {
        _url += "/ip_addresses";
        ApiKey = apiKey;
        var options = new RestClientOptions(_url);
        _client = new RestClient(options);
    }

    public async Task<IpAnalysisResult> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{ipAddress}").AddHeader("x-apikey", ApiKey);

        RestResponse<IpAnalysisResult> response;

        if (cancellationToken is not null)
        {
            response = await _client.ExecuteGetAsync<IpAnalysisResult>(request, cancellationToken.Value);
        }
        else
        {
            response = await _client.ExecuteGetAsync<IpAnalysisResult>(request);
        }

        if (response is { IsSuccessful: true, Data: not null })
        {
            return response.Data;
        }

        var errorContent = response.Content!;
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent)!;
        ThrowErrorResponseException(errorResponse);
        return new IpAnalysisResult();
    }
}