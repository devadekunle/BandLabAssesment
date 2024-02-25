using BandLabAssesment.Domain;
using BandLabAssesment.Models;

namespace BandLabAssesment.Mappers;

public static class CommentMapper
{
    public static Comment MapToDomain(this CommentDto comment, string postId)
    {
        return new Comment(postId, comment.Content, comment.CreatorId, comment.Creator);
    }
}