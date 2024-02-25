using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        Id = Guid.NewGuid().ToString();
        CreatorId = creatorId;
        Creator = creator;
        OriginalImageUrl = originalImageUrl;
        ResizedImageUrl = resizedImageUrl;
        CreatedAt = DateTime.UtcNow;
        _comments = comments ?? new();
    }

    public string Caption { get; set; }

    public string OriginalImageUrl { get; set; }
    public string ResizedImageUrl { get; set; }

    public string CreatorId { get; set; }

    public string Creator { get; set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyList<Comment> Comments { get => _comments.AsReadOnly(); }

    public void AddComment(Comment comment) => _comments.Add(comment);
}