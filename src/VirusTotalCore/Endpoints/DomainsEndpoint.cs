using System.Text.Json;
using RestSharp;
using VirusTotalCore.Models.Analysis.Domains;
using VirusTotalCore.Models.Analysis.IP;

namespace VirusTotalCore.Endpoints;

public class DomainsEndpoint(string apiKey) : BaseEndpoint(apiKey, "/domains")
{
    /*TODO: implement Get a domain report, Get comments on a domain, Add a comment to a domain
    Get objects related to a domain, Get object descriptors related to a domain, Get a DNS resolution object
    Get votes on a domain, Add a vote to a domain*/

    public async Task<DomainAnalysisReport> GetReport(string domain, CancellationToken? cancellationToken)
    {
        var request = new RestRequest($"/{domain}").AddHeader("x-apikey", ApiKey);

        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is not { IsSuccessful: true }) throw HandleError(restResponse.Content!);

        var resultJsonDocument = JsonDocument.Parse(restResponse.Content!);
        var result = resultJsonDocument.RootElement.GetProperty("data").Deserialize<DomainAnalysisReport>(JsonSerializerOptions)!;
        return result;
    }

    public void GetComments()
    {
        throw new NotImplementedException();
    }

    public void AddComment()
    {
        throw new NotImplementedException();
    }

    public void GetObjectsRelated()
    {
        throw new NotImplementedException();
    }

    public void GetObjectDescriptors()
    {
        throw new NotImplementedException();
    }

    public void GetDnsResolution()
    {
        throw new NotImplementedException();
    }

    public void GetVotes()
    {
        throw new NotImplementedException();
    }

    public void AddVote()
    {
        throw new NotImplementedException();
    }
}