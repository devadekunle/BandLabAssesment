using System;

namespace BandLabAssesment.Domain;

public class Comment : BaseEntity
{
    public Comment(string postId, string content, string creatorId, string creator)
    {
        ArgumentNullException.ThrowIfNull(postId);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(creator);
        ArgumentNullException.ThrowIfNull(creatorId);

        Id = Guid.NewGuid().ToString();
        PostId = postId;
        Content = content;
        CreatorId = creatorId;
        Creator = creator;
        CreatedAt = DateTime.UtcNow;
    }

    public string Content { get; set; }
    public string PostId { get; set; }

    public string CreatorId { get; set; }

    public string Creator { get; set; }

    public DateTime CreatedAt { get; private set; }
}