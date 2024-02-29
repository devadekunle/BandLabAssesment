using BandLabAssesment.Functions.V1;
using BandLabAssesment.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BandLabAssesment.Tests.Fixtures;

public class CreatePostFixture
{
    private HttpRequest _request;
    private IActionResult _response;
    private string _lastSavedPostId;
    private readonly string CreatorId = Ulid.NewUlid().ToString();
    private readonly string Creator = "Uzumaki Naruto";
    private readonly TestInitializer _initializer;
    private readonly CreatePost _sut;
    private readonly ILogger _logger;

    public CreatePostFixture(TestInitializer initializer)
    {
        _initializer = initializer;
        _sut = initializer.ServiceProvider.GetRequiredService<CreatePost>();
        _logger = initializer.ServiceProvider.GetRequiredService<ILogger>();
    }

    public void GivenAnInvalidPostWithNoImage()
    {
        var requestBuilder = new MultipartFormRequestBuilder();
        _request = requestBuilder
            .SetCreator(Creator)
            .SetCreatorId(CreatorId)
            .SetCaption("This is a test caption")
            .Build();
    }

    public async Task WhenTheEndpointIsCalled()
        => _response = await _sut.Run(_request, _logger, CancellationToken.None);

    public void ThenABadRequestReponseIsReturned()
    {
        var badRequestResponse = _response as BadRequestObjectResult;
        badRequestResponse.Should().NotBeNull();
        badRequestResponse.Value.Should().BeEquivalentTo(Constants.ErrorMessages.ImageNotProvided);
    }

    public void GivenAValidRequest()
    {
        var requestBuilder = new MultipartFormRequestBuilder();
        _request = requestBuilder
            .SetCreator(Creator)
            .SetCreatorId(CreatorId)
            .SetImage("./Data/Images/test-image.jpg", "image/jpeg")
            .SetCaption("This is a test caption")
            .Build();
    }

    public void ThenASuccessResponseIsReturned()
    {
        var okResponse = _response as OkObjectResult;
        okResponse.Should().NotBeNull();

        var responseBody = (PostCreated)okResponse.Value;
        responseBody.PostId.Should().NotBeNull();
        _lastSavedPostId = responseBody.PostId;
    }

    public async Task AndPostIsCreatedInDatabase()
    {
        var post = await _initializer.PostsContainer.ReadItemAsync<dynamic>(_lastSavedPostId, new(CreatorId));
        post.Should().NotBeNull();
    }
}