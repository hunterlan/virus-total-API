using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class DomainTest
{
    private const string GoogleDomain = "google.com";
    private const string GraphRelationship = "graphs";
    private string ApiKey { get; }
    private readonly DomainsEndpoint _endpoint;

    public DomainTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
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
        var comments = await _endpoint.GetComments(GoogleDomain, new CancellationToken(), null, 10);
        Assert.True(comments.Comments.Count() is 10);
    }
    
    [Fact]
    public async Task AddCommentTest()
    {
        var cancellationToken = new CancellationToken();
        var commentEndpoint = new CommentEndpoint(ApiKey);
        const string comment = "It's google and it's safe";
        var commentResult = await _endpoint.AddComment(GoogleDomain, comment, cancellationToken);
        try
        {
            Assert.True(string.Equals(commentResult.Attributes.Text, comment));
        }
        finally
        {
            await commentEndpoint.Delete(commentResult.Id, cancellationToken);   
        }
    }
    
    [Fact]
    public async Task GetRelationshipsTest()
    {
        var relatedObjectsJson = await _endpoint.GetRelatedObjects(GoogleDomain, GraphRelationship, null, null);
        Assert.True(!string.IsNullOrEmpty(relatedObjectsJson));
    }

    [Fact]
    public async Task GetDescriptorsTest() 
    {
        var descriptorsJson = await _endpoint.GetRelatedDescriptors(GoogleDomain, GraphRelationship, null, null);
        Assert.True(!string.IsNullOrEmpty(descriptorsJson));
    }
    
    /*
     * TODO: Write test for posting vote
     * Find a way to delete the vote
     */
}