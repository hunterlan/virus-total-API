using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class DomainTest
{
    private const string IpAddress = "8.8.8.8";
    private string ApiKey { get; }
    private readonly DomainsEndpoint _endpoint;

    public DomainTest()
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new DomainsEndpoint(ApiKey);
    }

    [Fact]
    public async Task TestDomainReport()
    {
        const string domain = "pja.edu.pl";
        var report = await _endpoint.GetReport("pja.edu.pl", new CancellationToken());
        Assert.True(report is {Type: "domain", Id: domain});
    }

    [Fact]
    public async Task TestGetCommentsDomainReport()
    {
        const string domain = "google.com";
        var comments = await _endpoint.GetComments(domain, new CancellationToken(), null, 10);
        Assert.True(comments.Comments.Length is 10);
    }
    
    [Fact]
    public async Task AddCommentTest()
    {
        var cancellationToken = new CancellationToken();
        var commentEndpoint = new CommentEndpoint(ApiKey);
        const string domain = "google.com";
        const string comment = "It's google and it's safe";
        var commentResult = await _endpoint.AddComment(domain, comment, cancellationToken);
        Assert.True(string.Equals(commentResult.Attributes.Text, comment));
        await commentEndpoint.Delete(commentResult.Id, cancellationToken);
    }
    
    /*
     * TODO: Write test for posting vote
     * Find a way to delete the vote
     */
}