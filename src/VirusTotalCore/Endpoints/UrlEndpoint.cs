using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Analysis;
using VirusTotalCore.Models.Analysis.URL;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Shared;

namespace VirusTotalCore.Endpoints;

public class UrlEndpoint(string apiKey) : BaseEndpoint(apiKey, "/urls")
{
    private async Task<string> Scan(string url, CancellationToken? cancellationToken)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.virustotal.com/api/v3/urls");
        request.Headers.Add("x-apikey", ApiKey);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(url), "url");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var resultString = await response.Content.ReadAsStringAsync();
        var resultJsonDocument = JsonDocument.Parse(resultString);

        //TODO: bug of RestSharp???
        /*var request = new RestRequest("/urls", Method.Post)
        {
            AlwaysMultipartFormData = true
        };
        request.AddParameter("url", "https://shields.io/badges/git-hub-actions-workflow-status");

        var restResponse = await PostFormResponse(request, cancellationToken);
        
        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);*/
        var result = resultJsonDocument.RootElement.GetProperty("data").GetProperty("id")
            .Deserialize<string>(JsonSerializerOptions)!;
        return result;
    }

    public async Task<AnalysisReport<UrlReportAttributes>> GetReport(string url, CancellationToken? cancellationToken)
    {
        var id = await Scan(url, cancellationToken);
        //TODO: When VirusTotal fix bug, change to ID
        var request = new RestRequest($"/{ToBase64String(url)}");
        var restResponse = await GetResponse(request, cancellationToken);
        
        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data")
            .Deserialize<AnalysisReport<UrlReportAttributes>>(JsonSerializerOptions)!;
        return result;
    }

    public void Rescan(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<CommentData> GetComments(string id, string? filter, string? cursor, CancellationToken? cancellationToken,
        int limit = 10)
    {
        var parameters = new { limit, filter, cursor };
        
        var request = new RestRequest($"/{id}/comments").AddObject(parameters);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.Deserialize<CommentData>(JsonSerializerOptions)!;
        return result;
    }

    public async Task<Comment> AddComment(string id, string comment, CancellationToken? cancellationToken)
    {
        var newComment = new AddComment
        {
            Data = new AddData<AddCommentAttribute>
            {
                Type = "comment",
                Attributes = new AddCommentAttribute
                {
                    Text = comment
                }
            }
        };
        
        var serializedJson = JsonSerializer.Serialize(newComment, JsonSerializerOptions);
        var request = new RestRequest($"/{id}/comments")
            .AddJsonBody(serializedJson);

        var restResponse = await PostResponse(request, cancellationToken);
        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);
        
        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<Comment>(JsonSerializerOptions)!;
        return result;
    }

    public void GetObjectsRelated()
    {
        throw new NotImplementedException();
    }

    public void GetObjectDescription()
    {
        throw new NotImplementedException();
    }

    public void GetVotes()
    {
        throw new NotImplementedException();
    }

    public void AddVote()
    {
        throw new NotImplementedException();
    }

    private static string ToBase64String(string plainText) 
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}