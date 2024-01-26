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
        var comments = await _endpoint.GetComments(domain, new CancellationToken(), 10, null);
        Assert.True(comments.Data.Length is 10);
    }
}