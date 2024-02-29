using BandLabAssesment.Models;
using BandLabAssesment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BandLabAssesment.Functions.V1;

public class AddPostComment
{
    private readonly PostsService _postsService;

    public AddPostComment(PostsService postsService)
    {
        _postsService = postsService;
    }

    [FunctionName(Constants.Functions.CreatePostCommentV1)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = Constants.ApiRoutes.CreatePostCommentV1)][FromBody] AddCommentToPostRequest payload,
        string postId,
        ILogger log,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = ValidateRequest(payload);
            if (!validationResult.IsValid)
                return new BadRequestResult();

            var commentId = await _postsService.CommentOnPost(payload, postId, cancellationToken);

            return new OkObjectResult(new CommentCreated(commentId));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundResult();
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Something went wrong while processing this request");
            return new InternalServerErrorResult();
        }
    }

    private static (bool IsValid, string ValidationError) ValidateRequest(AddCommentToPostRequest comment)
    {
        if (comment is null)
            return (false, "Requst payload cannot be null");

        if (string.IsNullOrEmpty(comment.Creator))
            return (false, "Creator must be provided");

        if (string.IsNullOrEmpty(comment.CreatorId))
            return (false, "CreatorId must be provided");

        if (string.IsNullOrEmpty(comment.Content))
            return (false, "Content must be provided");

        return (true, default);
    }
}