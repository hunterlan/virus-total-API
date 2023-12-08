using System.Text.Json;
using RestSharp;
using VirusTotalAPI.Models;
using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalAPI.Endpoints.IPAddress;

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

        RestResponse<IpAnalysisResult> response;

        if (cancellationToken is not null)
        {
            response = await Client.ExecuteGetAsync<IpAnalysisResult>(request, cancellationToken.Value);
        }
        else
        {
            response = await Client.ExecuteGetAsync<IpAnalysisResult>(request);
        }

        if (response is { IsSuccessful: true, Data: not null })
        {
            return response.Data;
        }

        var errorContent = response.Content!;

        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>()!;
        ThrowErrorResponseException(errorResponse);
        return new IpAnalysisResult();
    }
}