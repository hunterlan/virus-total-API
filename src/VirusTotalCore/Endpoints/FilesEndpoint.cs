using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Analysis;

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

    public Task<AnalysisResult> PostFile(string pathToFile, string? password, CancellationToken? cancellationToken)
    {
        return PostFile(pathToFile, Client.Options.BaseUrl!.ToString(), password, cancellationToken);
    }

    public async Task<AnalysisResult> PostFile(string pathToFile, string url, string? password, CancellationToken? cancellationToken)
    {
        cancellationToken ??= new CancellationToken();

        var localClient = new RestClient(url);
        var request = new RestRequest("")
                    .AddHeader("x-apikey", ApiKey)
                    .AddFile("file", pathToFile, "multipart/form-data");
        
        if (password is not null)
        {
            request.AddJsonBody(JsonSerializer.Serialize(password));
        }

        var restResponse = await localClient.ExecutePostAsync(request, cancellationToken.Value);
        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var analysisResult = resultJsonDocument.RootElement.GetProperty("data").Deserialize<AnalysisResult>(JsonSerializerOptions)!;
        
        return analysisResult;
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