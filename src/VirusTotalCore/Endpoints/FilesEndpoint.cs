using System.Security.Cryptography;
using System.Text.Json;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.File;

namespace VirusTotalCore.Endpoints;

/// <summary>
/// Send files for scanning and get report about them.
/// </summary>
/// <param name="apiKey">User's API key.</param>
public class FilesEndpoint : BaseEndpoint
{
    public FilesEndpoint(string apiKey) : base(apiKey, "files") { }
    public FilesEndpoint(HttpClient customHttpClient, string apiKey) : base(customHttpClient, apiKey, "files") { }
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
        var url = HttpClient.BaseAddress! + CurrentEndpointName;

        if (File.Exists(pathToFile))
        {
            var fileInfo = new FileInfo(pathToFile);
            var fileSizeBytes = fileInfo.Length;

            if (fileSizeBytes > MaxSmallSizeBytes)
            {
                //https://github.com/aio-libs/aiohttp/issues/4678
                //Exception: Malformed multipart body.
                url = await GetUrlForPost(cancellationToken);
            }
        }
        else
        {
            throw new FileNotFoundException($"Unable to find the specified file. Path is {pathToFile}");
        }

        await using var sendStream = File.OpenRead(pathToFile);
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        using var content = new MultipartFormDataContent();

        content.Add(new StreamContent(sendStream), "file", Path.GetFileName(sendStream.Name));
        if (password is not null)
        {
            content.Add(new StringContent(password), "password");    
        }
        
        requestMessage.Content = content;
    
        using var response = await HttpClient.SendAsync(requestMessage);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken.Value);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var sha256 = SHA256.Create();
        byte[] hashBytes;
        await using (var localStream = File.OpenRead(pathToFile))
        {
            hashBytes = await sha256.ComputeHashAsync(localStream);
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
        cancellationToken ??= new CancellationToken();
        var requestUrl = "upload_url";
        const string rootPropertyName = "data";
        
        using var response = await HttpClient.GetAsync(requestUrl, cancellationToken.Value);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken.Value);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.RootElement.GetProperty(rootPropertyName).GetString()!;

        return result;
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
        const string rootPropertyName = "data";
        return await GetAsync<AnalysisReport<FileReportAttributes>>(fileHash, rootPropertyName, cancellationToken ?? new CancellationToken());
    }
}