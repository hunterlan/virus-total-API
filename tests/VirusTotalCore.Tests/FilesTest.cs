using Microsoft.Extensions.Configuration;
using RestSharp;
using VirusTotalCore.Endpoints;
using Xunit.Abstractions;

namespace VirusTotalCore.Tests;

public class FilesTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private string ApiKey { get; }
    private FilesEndpoint Endpoint { get; }
    private const string TestFileHashId = "80e211f190a08c4a28da7c85fbd26b82";
    private const string GraphRelationship = "graphs";
    public FilesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
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
        _testOutputHelper.WriteLine(Directory.GetCurrentDirectory());
        var analysisResult = await Endpoint.PostFile($"test_files{Path.DirectorySeparatorChar}123.txt", null, new CancellationToken());
        Assert.True(analysisResult is not null);
    }

    [Fact]
    public async Task GetReportTest()
    {
        var report = await Endpoint.GetReport(TestFileHashId, null);
        Assert.True(report is not null);
    }
    
    [Fact]
    public async Task GetRelationshipsTest()
    {
        var relatedObjectsJson = await Endpoint.GetRelatedObjects(TestFileHashId, GraphRelationship, null, null);
        Assert.True(!string.IsNullOrEmpty(relatedObjectsJson));
    }
}