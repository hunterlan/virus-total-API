using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;

namespace VirusTotalCore.Tests;

public class FilesTest
{
    private string ApiKey { get; }
    private FilesEndpoint Endpoint { get; }

    public FilesTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        ApiKey = settings["apiKey"]!;
        Endpoint = new FilesEndpoint(ApiKey);
    }

    [Fact]
    public async Task PostSmallFile()
    {
        var analysisResult = await Endpoint.PostFile($"TestFiles{Path.PathSeparator}123.txt", null, new CancellationToken());
        Assert.True(analysisResult is not null);
    }

    [Fact]
    public async Task GetReportTest()
    {
        var report = await Endpoint.GetReport("80e211f190a08c4a28da7c85fbd26b82", null);
        Assert.True(report is not null);
    }
}