using BandLabAssesment.Domain;
using BandLabAssesment.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Functions.v1;

public class UpdatePostWithComments
{
    private readonly PostsService _postsService;

    public UpdatePostWithComments(PostsService postsService)
    {
        _postsService = postsService;
    }

    [FunctionName(Constants.Functions.UpdatePostWithComments)]
    public async Task Run([CosmosDBTrigger(
        databaseName: "%CosmosDb:DatabaseName%",
        containerName:"comments",
        Connection = "CosmosDb:ConnectionString",
        LeaseContainerPrefix = "updatePostComments",
        StartFromBeginning = true,
        CreateLeaseContainerIfNotExists = true)] IReadOnlyList<Comment> comments,
        CancellationToken token,
        ILogger logger)
    {
        try
        {
            foreach (var comment in comments)
            {
                if (!comment.IsDeleted)
                {
                    await _postsService.UpdatePostWithLatestCommentDetails(comment, token);
                }
                else
                {
                    await _postsService.CascadeCommentDeletiontoPost(comment, token);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Something went wrong updating posts with comment");
        }
    }
}