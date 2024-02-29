using BandLabAssesment.Services;
using Microsoft.AspNetCore.Http;
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

public class DeleteComment
{
    private readonly PostsService _postsService;

    public DeleteComment(PostsService postsService)
    {
        _postsService = postsService;
    }

    [FunctionName(Constants.Functions.DeleteCommentV1)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "DELETE", Route = Constants.ApiRoutes.DeletePostCommentV1)] HttpRequest req,
        string commentId,
        ILogger log,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = ValidateRequest(commentId);
            if (!validationResult.IsValid)
                return new BadRequestResult();

            await _postsService.DeleteComment(commentId, cancellationToken);

            return new NoContentResult();
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

    private static (bool IsValid, string Message) ValidateRequest(string commentId)
    {
        if (string.IsNullOrWhiteSpace(commentId))
        {
            return (false, "CreatorId is required");
        }
        return (true, default);
    }
}