using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.File;

namespace VirusTotalCore.Endpoints;

public class FilesEndpoint(string apiKey) : BaseEndpoint(apiKey, "/files")
{
    private const int MaxSmallSizeBytes = 33554432;
    public async Task<string> PostFile(string pathToFile, string? password,
        CancellationToken? cancellationToken)
    {
        cancellationToken ??= new CancellationToken();
        var url = Client.Options.BaseUrl!.ToString();

        if (File.Exists(pathToFile))
        {
            // Create a FileInfo object for the file
            var fileInfo = new FileInfo(pathToFile);

            // Get the file size in bytes
            var fileSizeBytes = fileInfo.Length;

            if (fileSizeBytes > MaxSmallSizeBytes)
            {
                url = await GetUrlForPost(cancellationToken);
            }
        }
        else
        {
            throw new FileNotFoundException();
        }

        var localClient = new RestClient(url);
        var request = new RestRequest("")
            .AddHeader("x-apikey", ApiKey)
            .AddFile("file", pathToFile, "multipart/form-data");

        if (password is not null) request.AddJsonBody(JsonSerializer.Serialize(password));

        var restResponse = await localClient.ExecutePostAsync(request, cancellationToken.Value);
        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var analysisResult = resultJsonDocument.RootElement.GetProperty("data").GetProperty("id")
            .Deserialize<string>(JsonSerializerOptions)!;

        return analysisResult;
    }

    private async Task<string> GetUrlForPost(CancellationToken? cancellationToken)
    {
        var request = new RestRequest("/upload_url");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        return resultJsonDocument.RootElement.GetProperty("data").GetString()!;
    }

    public async Task<AnalysisReport<FileReportAttributes>> GetReport(string fileHash, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{fileHash}");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var reportResult = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AnalysisReport<FileReportAttributes>>(JsonSerializerOptions)!;
        return reportResult;
    }
}