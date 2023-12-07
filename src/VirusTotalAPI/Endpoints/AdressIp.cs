using RestSharp;
using RestSharp.Authenticators;

namespace VirusTotalAPI.Endpoints;

public class AdressIp : Endpoint
{
    public AdressIp(string apiKey)
    {
        _url += "/ip_addresses";
        ApiKey = apiKey;
        var options = new RestClientOptions(_url);
        _client = new RestClient(options);
    }

    public void GetReport(string ipAddress)
    {
        
    }
}