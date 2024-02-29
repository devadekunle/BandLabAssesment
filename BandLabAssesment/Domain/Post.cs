using System;
using System.Collections.Generic;
using System.Linq;

namespace BandLabAssesment.Domain;

public class Post : BaseEntity
{
    private List<Comment> _comments;

    public Post(string creatorId, string creator, string originalImageUrl, string resizedImageUrl, List<Comment> comments = null)
    {
        ArgumentNullException.ThrowIfNull(creatorId);
        ArgumentNullException.ThrowIfNull(originalImageUrl);
        ArgumentNullException.ThrowIfNull(resizedImageUrl);
        ArgumentNullException.ThrowIfNull(creator);

        Id = Id = $"{Ulid.NewUlid()}-{creatorId}";
        CreatorId = creatorId;
        Creator = creator;
        OriginalImageUrl = originalImageUrl;
        ResizedImageUrl = resizedImageUrl;
        CreatedAt = DateTime.UtcNow;
        _comments = comments ?? new();
    }

    public string Caption { get; set; }

    public int TotalCommentCount { get; set; }

    public string OriginalImageUrl { get; set; }
    public string ResizedImageUrl { get; set; }

    public string CreatorId { get; set; }

    public string Creator { get; set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyList<Comment> Comments { get => _comments.AsReadOnly(); }

    public void AddLatestComment(Comment comment)
    {
        if (Comments.Count < 2)
            _comments.Add(comment);
        else
        {
            var orderedComments = _comments.OrderBy(e => CreatedAt).ToList();
            orderedComments[0] = comment;

            _comments = orderedComments.OrderByDescending(e => e.CreatedAt).ToList();
        }
    }

    public void IncreamentCommentCount() => TotalCommentCount++;

    public bool RemoveFromLatestComments(Comment comment)
    {
        return _comments.Remove(comment);
    }
}