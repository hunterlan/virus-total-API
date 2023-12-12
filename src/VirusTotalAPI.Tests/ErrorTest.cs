
using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints;

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
        ApiKey = "";
        _endpoint = new AddressIpEndpoint(ApiKey);
    }
    
    //https://stackoverflow.com/questions/45017295/assert-an-exception-using-xunit
    [Fact]
    public async Task IncorrectApiKeyAssign()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _endpoint.GetReport("1.1.1.1", null));
        Assert.Equal("Api key shouldn't be empty.", exception.Message);
    }
}