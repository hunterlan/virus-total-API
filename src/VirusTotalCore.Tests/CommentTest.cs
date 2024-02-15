using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class CommentTest
{
    private string ApiKey { get; }
    private readonly CommentEndpoint _endpoint;

    public CommentTest()
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new CommentEndpoint(ApiKey);
    }

    [Fact]
    public async Task GetLatestCommentTest()
    {
        var comments = await _endpoint.GetLatest(null, null, new CancellationToken());
        Assert.True(comments.Data.Length is 10 && comments.Meta.Cursor is not null && comments.Links.Next is not null);
    }
}