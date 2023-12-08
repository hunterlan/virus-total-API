using VirusTotalAPI.Exceptions;

namespace VirusTotalAPI.Tests;

public class IpAddressTest : Test
{
    
    [Fact]
    public async Task IncorrectIpAddressReport()
    {
        await Assert.ThrowsAsync<InvalidArgumentException>(() =>Endpoint.GetReport("", null));
    }
}