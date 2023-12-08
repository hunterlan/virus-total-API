using Microsoft.Extensions.Configuration;
using VirusTotalAPI.Endpoints.IPAddress;

namespace VirusTotalAPI.Tests;

public class Test
{
    private string ApiKey { get; set; }
    protected readonly AddressIpEndpoint Endpoint;

    protected Test()
    {
        var settings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        Endpoint = new AddressIpEndpoint(ApiKey);
    }
}