using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints;
using VirusTotalAPI.Exceptions;
using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalAPI.Tests;

public class IpAddressTest
{
    private const string IpAddress = "8.8.8.8";
    private string ApiKey { get; }
    private readonly AddressIpEndpoint _endpoint;

    public IpAddressTest()
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new AddressIpEndpoint(ApiKey);
    }

    [Fact]
    public async Task IncorrectIpAddressReport()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _endpoint.GetReport("", null));
    }

    [Fact]
    public async Task IpAddressReport()
    {
        var report = await _endpoint.GetReport(IpAddress, new CancellationToken());
        Assert.True(report is { Id: IpAddress, Type: "ip_address" });
    }

    [Fact]
    public async Task IpAddressComments()
    {
        var ipComment = await _endpoint.GetComments(IpAddress, null, new CancellationToken());
        Assert.True(ipComment.Data.Length is 10);
    }
}