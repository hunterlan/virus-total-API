using System.Text.Json;
using RestSharp;

namespace VirusTotalCore.Endpoints;

public class FilesEndpoint : Endpoint
{
    public FilesEndpoint(string apiKey)
    {
        Url += "/files";
        ApiKey = apiKey;
        var options = new RestClientOptions(Url);
        Client = new RestClient(options);
    }

    public void PostFile(string pathToFile, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void PostFile(string pathToFile, string Url, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUrlForPost(CancellationToken? cancellationToken)
    {
        var request = new RestRequest("/upload_url").AddHeader("x-apikey", ApiKey);
        
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        return resultJsonDocument.RootElement.GetProperty("data").GetString()!;
    }
}