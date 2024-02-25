using BandLabAssesment.Mappers;
using BandLabAssesment.Models;
using BandLabAssesment.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Services;

public class PostsService
{
    private readonly PostRepository _postsRepository;
    private readonly CommentRepository _commentRepository;

    public PostsService(PostRepository postsRepository, CommentRepository commentRepository)
    {
        _postsRepository = postsRepository;
        _commentRepository = commentRepository;
    }

    public Task CreatePost(PostDto post, CancellationToken token)
        => _postsRepository.UpsertAsync(post.MapToDomain(), post.UserId, token);

    public Task CommentOnPost(CommentDto comment, string postId, CancellationToken token)
        => _commentRepository.UpsertAsync(comment.MapToDomain(postId), comment.CreatorId, token);
}