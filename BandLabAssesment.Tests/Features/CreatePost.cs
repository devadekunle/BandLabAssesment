using BandLabAssesment.Tests.Fixtures;
using TestStack.BDDfy;

namespace BandLabAssesment.Tests.Features;

[Collection(IntegrationTestCollection.Name)]
[Story(AsA = "User",
    IWant = "To be able to create posts with images (1 post - 1 image")]
public class CreatePost
{
    private readonly CreatePostFixture steps;

    public CreatePost(TestInitializer initializer)
        => steps = new CreatePostFixture(initializer);

    [Fact]
    public Task CreatePostEndpoint_ReturnsBadRequest_WhenNoImageProvidedInRequest()
    {
        this.Given(s => steps.GivenAnInvalidPostWithNoImage())
            .When(s => steps.WhenTheEndpointIsCalled())
            .Then(s => steps.ThenABadRequestReponseIsReturned())
            .BDDfy();

        return Task.CompletedTask;
    }

    [Fact]
    public Task CreatePostEndpoint_ReturnsSuccess_WhenRequestIsValid()
    {
        this.Given(s => steps.GivenAValidRequest())
            .When(s => steps.WhenTheEndpointIsCalled())
            .Then(s => steps.ThenASuccessResponseIsReturned())
            .And(s => steps.AndPostIsCreatedInDatabase())
            .BDDfy();

        return Task.CompletedTask;
    }
}