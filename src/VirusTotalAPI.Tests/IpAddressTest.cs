using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints;
using VirusTotalAPI.Exceptions;
using VirusTotalAPI.Models.Analysis.IP;
using VirusTotalAPI.Models.Comments.IP;

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

    [Fact]
    public async Task IpAddressVotes()
    {
        var ipVotes = await _endpoint.GetVotes(IpAddress, new CancellationToken());
        Assert.True(ipVotes.Data.Length > 0 && ipVotes.Data[0].Attributes is not null);
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