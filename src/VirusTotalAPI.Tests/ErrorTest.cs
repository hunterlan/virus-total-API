
using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints.IPAddress;

namespace VirusTotalAPI.Tests;

public class ErrorTest : Test
{

    [Fact]
    public async Task IncorrectApiKeyAssign()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => Endpoint.GetReport("1.1.1.1", null));
        Assert.Equal("Api key shouldn't be empty.", exception.Message);
    }
}