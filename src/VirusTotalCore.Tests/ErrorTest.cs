
using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints;
using VirusTotalAPI.Exceptions;

namespace VirusTotalAPI.Tests;

public class ErrorTest
{
    private string ApiKey { get; }
    private readonly AddressIpEndpoint _endpoint;

    public ErrorTest()
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new AddressIpEndpoint(ApiKey);
    }

    [Fact]
    public void IncorrectApiKeyAssign()
    {
        Assert.Throws<ArgumentException>(() => new AddressIpEndpoint(""));
    }
}