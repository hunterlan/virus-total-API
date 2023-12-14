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
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        Endpoint = new FilesEndpoint(ApiKey);
    }

    [Fact]
    public async Task GetUrlTest()
    {
        var url = await Endpoint.GetUrlForPost(null);
        Assert.True(!string.IsNullOrWhiteSpace(url));
    }
}