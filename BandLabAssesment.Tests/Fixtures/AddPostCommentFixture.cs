using BandLabAssesment.Domain;
using BandLabAssesment.Extensions;
using BandLabAssesment.Functions.V1;
using BandLabAssesment.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BandLabAssesment.Tests.Fixtures;

public class AddPostCommentFixture
{
    private AddCommentToPostRequest _request;
    private string _existingPostId;
    private string _createdCommentId;
    private readonly ILogger _logger;
    private readonly TestInitializer _initializer;
    private readonly AddPostComment _sut;
    private IActionResult _response;

    public AddPostCommentFixture(TestInitializer initializer)
    {
        _initializer = initializer;
        _sut = initializer.ServiceProvider.GetRequiredService<AddPostComment>();
        _logger = initializer.ServiceProvider.GetRequiredService<ILogger>();
    }

    public async Task GivenAPostExists()
    {
        var post = new Post(
            Ulid.NewUlid().ToString(),
            "Uzumaki Boruto",
            "http://originalimageurl.com",
            "http://resizedimageurl.com");

        await _initializer.PostsContainer.UpsertItemAsync(post, new PartitionKey(post.Id.ResolvePartitionKeyFromId()));
        _existingPostId = post.Id;
    }

    public void AndAValidRequestToAddAComment()
        => _request = new AddCommentToPostRequest
        {
            Content = "this is a test comment",
            Creator = "Uchiha Sasuke",
            CreatorId = Ulid.NewUlid().ToString()
        };

    public async Task WhenTheRequestIsMade()
        => _response = await _sut.Run(_request, _existingPostId, _logger, CancellationToken.None);

    public void ThenASuccessResponseIsReturned()
    {
        var okResponse = _response as OkObjectResult;

        okResponse.Should().NotBeNull();

        var responseBody = okResponse.Value as CommentCreated;
        responseBody.Should().NotBeNull();
        _createdCommentId = responseBody.CommentId;
    }

    public async Task AndACommentIsCreated()
    {
        var comment = await _initializer.CommentsContainer.ReadItemAsync<dynamic>(_createdCommentId,
            new(_createdCommentId.ResolvePartitionKeyFromId()));
        comment.Should().NotBeNull();
    }
}