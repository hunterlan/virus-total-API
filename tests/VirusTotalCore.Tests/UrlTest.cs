using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class UrlTest
{
    private string ApiKey { get; }
    private readonly UrlEndpoint _endpoint;

    public UrlTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new UrlEndpoint(ApiKey);
    }

    [Fact]
    public async Task GetReportTest()
    {
        var urlReport = await _endpoint.GetReport("https://shields.io/badges/git-hub-actions-workflow-status", new CancellationToken());
        Assert.NotNull(urlReport);
    }
}