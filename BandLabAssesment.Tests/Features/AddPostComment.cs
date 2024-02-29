using BandLabAssesment.Tests.Fixtures;
using TestStack.BDDfy;

namespace BandLabAssesment.Tests.Features;

[Collection(IntegrationTestCollection.Name)]
[Story(AsA = "User",
    IWant = "To be able to comment on a post")]
public class AddPostComment
{
    private readonly AddPostCommentFixture steps;

    public AddPostComment(TestInitializer initializer)
        => steps = new AddPostCommentFixture(initializer);

    [Fact]
    public Task AddCommentToPostEndpoint_ReturnsSuccess_WithValidRequest()
    {
        this.Given(s => steps.GivenAPostExists())
            .And(s => steps.AndAValidRequestToAddAComment())
            .When(s => steps.WhenTheRequestIsMade())
            .Then(s => steps.ThenASuccessResponseIsReturned())
            .And(s => steps.AndACommentIsCreated())
            .BDDfy();

        return Task.CompletedTask;
    }
}