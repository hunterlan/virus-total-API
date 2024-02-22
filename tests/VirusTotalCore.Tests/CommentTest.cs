using Microsoft.Extensions.Configuration;
using VirusTotalCore.Endpoints;
using VirusTotalCore.Models.Comments;
using VirusTotalCore.Models.Comments.Vote;

namespace VirusTotalCore.Tests;

public class CommentTest
{
    private string ApiKey { get; }
    private readonly CommentEndpoint _endpoint;
    private readonly DomainsEndpoint _domainsEndpoint;

    public CommentTest()
    {
        var settings = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json")
            .Build();
        ApiKey = settings["apiKey"]!;
        _endpoint = new CommentEndpoint(ApiKey);
        _domainsEndpoint = new DomainsEndpoint(ApiKey);
    }

    [Fact]
    public async Task GetLatestCommentsTest()
    {
        var commentData = await _endpoint.GetLatest(null, null, new CancellationToken());
        Assert.True(commentData.Comments.Count() is 10 && commentData.Meta.Cursor is not null && commentData.Links.Next is not null);
    }

    [Fact]
    public async Task GetSpecificCommentTest()
    {
        var token = new CancellationToken();
        var exampleCommentData = await CreateComment(token);
        var realCommentData = await _endpoint.Get(exampleCommentData.Id, token);
        Assert.True(string.Equals(exampleCommentData.Id, realCommentData.Id) 
                    && string.Equals(exampleCommentData.Attributes.Text, realCommentData.Attributes.Text)
                    && exampleCommentData.Attributes.Date == realCommentData.Attributes.Date);
        await _endpoint.Delete(exampleCommentData.Id, token);
    }

    [Fact]
    public async Task AddVoteToComment()
    {
        var token = new CancellationToken();
        var commentData = await CreateComment(token);
        await _endpoint.AddVote(commentData.Id, CommentVerdict.Positive, token);
        var realCommentData = await _endpoint.Get(commentData.Id, token);
        Assert.True(realCommentData.Attributes.Votes.Positive is 1);
        await _endpoint.Delete(commentData.Id, token);
    }

    private async Task<Comment> CreateComment(CancellationToken cancellationToken)
    {
        const string domain = "pja.edu.pl";
        const string comment = "Polsko-Japońska Akademia Technik Komputerowych";
        var commentData = await _domainsEndpoint.AddComment(domain, comment, cancellationToken);
        return commentData;
    }
}