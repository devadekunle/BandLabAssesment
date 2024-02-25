using BandLabAssesment.Models;
using BandLabAssesment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
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
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.ApiRoutes.CreatePostCommentV1)] HttpRequest req,
        string postId,
        ILogger log,
        CancellationToken cancellationToken)
    {
        try
        {
            var comment = await ParseRequest(req);

            var validationResult = ValidateRequest(comment);
            if (!validationResult.IsValid)
                return new BadRequestResult();

            await _postsService.CommentOnPost(comment, postId, cancellationToken);

            return new OkResult();
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Something went wrong while processing this request");
            return new InternalServerErrorResult();
        }
    }

    private static async Task<CommentDto> ParseRequest(HttpRequest req)
    {
        var request = await new StreamReader(req.Body).ReadToEndAsync();
        var comment = JsonSerializer.Deserialize<CommentDto>(request, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        return comment;
    }

    private static (bool IsValid, string ValidationError) ValidateRequest(CommentDto comment)
    {
        return (true, default);
    }
}