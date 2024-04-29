using System.Net.Http.Json;
using System.Text.Json;
using RestSharp;
using VirusTotalCore.Enums;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.IP;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Votes;

namespace VirusTotalCore.Endpoints;

public class AddressIpEndpoint(string apiKey) : BaseEndpoint(apiKey, "ip_addresses/")
{
    /// <summary>
    ///     Get report on given ip address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="AddressReportAttributes">Analysis report</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<AnalysisReport<AddressReportAttributes>> GetReport(string ipAddress, CancellationToken? cancellationToken)
    {
        var response = await HttpClient.GetAsync(ipAddress, cancellationToken: cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync();
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }

        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AnalysisReport<AddressReportAttributes>>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    ///     Get comments for given IP address
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cursor"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="limit">Maximum number of comments to retrieve. Default is 10.</param>
    /// <returns cref="IpComment">Comments</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    /// <exception cref="AuthenticationRequiredError">Empty API key</exception>
    /// <exception cref="WrongCredentialsError">Invalid API key</exception>
    public async Task<CommentData> GetComments(string ipAddress, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var requestUrl = $"{ipAddress}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }

        var response = await HttpClient.GetAsync(requestUrl, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync();
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.Deserialize<CommentData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    ///     Post your comment to IP address.
    ///     Any word starting with # in your comment's text will be considered a tag,
    ///     and added to the comment's tag attribute.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="comment">Your comment for given IP address</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    /// <exception cref="AlreadyExistsException">Comment with given content is already exists.</exception>
    public async Task AddComment(string ipAddress, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment(comment);
        var requestUrl = $"{ipAddress}/comments";

        var response = await HttpClient.PostAsJsonAsync(requestUrl, newComment, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync();
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
    }

    /// <summary>
    ///     Get community votes for given IP address.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param name="cancellationToken"></param>
    /// <returns cref="VoteData">IP address community votes</returns>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task<VoteData> GetVotes(string ipAddress, CancellationToken? cancellationToken)
    {
        /*var requestUrl = $"/{ipAddress}/votes";

        var request = new RestRequest(requestUrl);
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<VoteData>(JsonSerializerOptions)!;
        return result;*/

        var requestUrl = $"{ipAddress}/votes";
        var response = await HttpClient.GetAsync(requestUrl, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync();
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.Deserialize<VoteData>(JsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    ///     Post your vote to IP Address.
    ///     The verdict attribute must have be either harmless or malicious.
    /// </summary>
    /// <param name="ipAddress">IPv4 address as string</param>
    /// <param cref="VerdictType" name="verdict">Harmless or malicious</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentOutOfRangeException">Only harmless or malicious verdict is available.</exception>
    /// <exception cref="NotFoundException">Given IP address not found.</exception>
    public async Task AddVote(string ipAddress, VerdictType verdict, CancellationToken? cancellationToken)
    {
        /*var newVote = new AddVote(verdict);

        var requestUrl = $"/{ipAddress}/votes";
        var serializedJson = JsonSerializer.Serialize(newVote, JsonSerializerOptions);
        var request = new RestRequest(requestUrl)
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);*/
        
        
        var newVote = new AddVote(verdict);
        var requestUrl = $"/{ipAddress}/votes";
        
        var response = await HttpClient.PostAsJsonAsync(requestUrl, newVote, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync();
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
    }
}