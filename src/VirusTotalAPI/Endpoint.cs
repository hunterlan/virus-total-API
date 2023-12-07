using RestSharp;

namespace VirusTotalAPI;

public abstract class Endpoint
{
    protected RestClient _client;
    protected string _url = "https://www.virustotal.com/api/v3";
    protected string ApiKey
    {
        get => ApiKey;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Api key shouldn't be empty.");
            }

            ApiKey = value;
        }
    }
}
