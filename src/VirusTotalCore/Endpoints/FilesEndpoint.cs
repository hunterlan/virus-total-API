using System.Security.Cryptography;
using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.File;

namespace VirusTotalCore.Endpoints;

/// <summary>
/// Send files for scanning and get report about them.
/// </summary>
/// <param name="apiKey">User's API key.</param>
public class FilesEndpoint(string apiKey) : BaseEndpoint(apiKey, "/files")
{
    /// <summary>
    /// Size of file is allowed to post without requesting an URL for it (32 MB in bytes).
    /// </summary>
    private const int MaxSmallSizeBytes = 33554432;
    /// <summary>
    /// Allows to send file for scanning.
    /// If file is less than 32 MB, it will use default URL.
    /// If file is bigger, than it will call function <see cref="GetUrlForPost"/> to get URL for sending huge file.
    /// </summary>
    /// <param name="pathToFile">File path.</param>
    /// <param name="password">Optional. Password to file.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>SHA256 hash for getting report.</returns>
    /// <exception cref="FileNotFoundException">File doesn't exists.</exception>
    /// <exception cref="Exception"></exception>
    public async Task<string> PostFile(string pathToFile, string? password,
        CancellationToken? cancellationToken)
    {
        cancellationToken ??= new CancellationToken();
        var url = Client.Options.BaseUrl!.ToString();

        if (File.Exists(pathToFile))
        {
            var fileInfo = new FileInfo(pathToFile);
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

        var sha256 = SHA256.Create();
        byte[] hashBytes;
        await using (var stream = File.OpenRead(pathToFile))
        {
            hashBytes = await sha256.ComputeHashAsync(stream);
        }
        
        return System.Text.Encoding.Default.GetString(hashBytes);
    }

    /// <summary>
    /// Get URL for sending a file with size more than 32 MB.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>URL for sending file.</returns>
    /// <exception cref="Exception"></exception>
    private async Task<string> GetUrlForPost(CancellationToken? cancellationToken)
    {
        var request = new RestRequest("/upload_url");

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        return resultJsonDocument.RootElement.GetProperty("data").GetString()!;
    }

    /// <summary>
    /// Get report about file.
    /// </summary>
    /// <param name="fileHash">Hash of file.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns cref="FileReportAttributes">Analysis report</returns>
    /// <exception cref="Exception"></exception>
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