
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

    [Fact]
    public async Task CatchErrorOnIncorrectPostComment()
    {
        await Assert.ThrowsAsync<BadRequestException>(() =>
             _endpoint.PostComment("8.8.8.8", "", new CancellationToken()));
    }

    [Fact]
    public async Task DuplicateErrorPostComment()
    {
        await Assert.ThrowsAsync<AlreadyExistsException>(() =>
            _endpoint.PostComment("8.8.8.8", "Lorem ipsum dolor sit ...", new CancellationToken()));
    }
}