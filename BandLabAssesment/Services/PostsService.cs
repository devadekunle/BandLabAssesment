using BandLabAssesment.Domain;
using BandLabAssesment.Mappers;
using BandLabAssesment.Models;
using BandLabAssesment.Persistence;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Services;

public class PostsService
{
    private readonly PostRepository _postsRepository;
    private readonly CommentRepository _commentRepository;
    private readonly ILogger _logger;

    public PostsService(PostRepository postsRepository, CommentRepository commentRepository, ILogger logger)
    {
        _postsRepository = postsRepository;
        _commentRepository = commentRepository;
        _logger = logger;
    }

    public async Task<string> CreatePost(PostDto post, CancellationToken token)
    {
        var newPost = post.MapToDomain();
        await _postsRepository.UpsertAsync(newPost, token);
        return newPost.Id;
    }

    public async Task<string> CommentOnPost(AddCommentToPostRequest comment, string postId, CancellationToken token)
    {
        var newComment = comment.MapToDomain(postId);
        await _commentRepository.UpsertAsync(newComment, token);
        return newComment.Id;
    }

    public async Task DeleteComment(string commentId, CancellationToken token)
    {
        var comment = await _commentRepository.GetById(commentId, token);
        comment.Delete();
        await _commentRepository.UpsertAsync(comment, token);
    }

    public async Task UpdatePostWithLatestCommentDetails(Comment comment, CancellationToken token)
    {
        var post = await _postsRepository.GetById(comment.PostId, token);
        if (post is null)
            _logger.LogError("Related post not found while processing comments change feed. Post Id: {PostId}", comment.PostId);

        post.AddLatestComment(comment);
        post.TotalCommentCount++;
        await _postsRepository.UpsertAsync(post, token);
    }

    public async Task CascadeCommentDeletiontoPost(Comment comment, CancellationToken token)
    {
        var post = await _postsRepository.GetById(comment.PostId, token);
        if (post is null)
            _logger.LogError("Related post not found while processing comments change feed. Post Id: PostId}", comment.PostId);

        post.TotalCommentCount--;

        var isCommentRemovedFromRecentComments = post.RemoveFromLatestComments(comment);
        if (isCommentRemovedFromRecentComments && post.Comments.Any())
        {
            var latestCommentSince = await _commentRepository.GetLatestCommentSince(post.Comments[0].CreatedAt);
            if (latestCommentSince is not null)
            {
                post.AddLatestComment(latestCommentSince);
            }
        }

        await _postsRepository.UpsertAsync(post, token);
    }
}