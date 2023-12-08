using VirusTotalAPI.Exceptions;
using VirusTotalAPI.Models.Analysis.IP;

namespace VirusTotalAPI.Tests;

public class IpAddressTest : Test
{
    
    [Fact]
    public async Task IncorrectIpAddressReport()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => Endpoint.GetReport("", null));
    }

    [Fact]
    public async Task IpAddressReport()
    {
        const string ipAddress = "8.8.8.8";
        
        var report = await Endpoint.GetReport(ipAddress, new CancellationToken());
        Assert.True(report is { Id: ipAddress, Type: "ip_address" });
    }
}