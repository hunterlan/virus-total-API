namespace VirusTotalCore.Endpoints;

public class UrlEndpoint(string apiKey) : BaseEndpoint(apiKey, "/urls")
{
    public void Scan(string url)
    {
        throw new NotImplementedException();
    }

    public void GetReport(string id)
    {
        throw new NotImplementedException();
    }

    public void Rescan(string id)
    {
        throw new NotImplementedException();
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

    public void GetObjectDescription()
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