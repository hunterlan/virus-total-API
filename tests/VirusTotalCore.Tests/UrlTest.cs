using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class UrlTest
{
    private string ApiKey { get; }
    private readonly UrlEndpoint _endpoint;
    private readonly CommentEndpoint _commentEndpoint;
    public UrlTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _commentEndpoint = new CommentEndpoint(ApiKey);
        _endpoint = new UrlEndpoint(ApiKey);
    }

    [Fact]
    public async Task GetReportTest()
    {
        var urlReport = await _endpoint.GetReport("https://shields.io/badges/git-hub-actions-workflow-status", new CancellationToken());
        Assert.NotNull(urlReport);
    }

    [Fact]
    public async Task GetCommentsTest()
    {
        string urlId = "aba51b6c10fd1449e5700fc8c022c53157247b32bce5e33217495b11d9aee78a";
        var commentData = await _endpoint.GetComments(urlId, null, null, null);
        Assert.NotNull(commentData);
    }

    [Fact]
    public async Task AddCommentTest()
    {
        string urlId = "aba51b6c10fd1449e5700fc8c022c53157247b32bce5e33217495b11d9aee78a";
        var commentData = await _endpoint.AddComment(urlId, "Website of Microsoft, which suggest to download dotnet.", null);
        Assert.NotNull(commentData);
        await _commentEndpoint.Delete(commentData.Id, null);
    }

    [Fact]
    public async Task GetVotesTest()
    {
        string urlId = "aba51b6c10fd1449e5700fc8c022c53157247b32bce5e33217495b11d9aee78a";
        var votesData = await _endpoint.GetVotes(urlId, new CancellationToken());
        Assert.NotNull(votesData);
    }
}