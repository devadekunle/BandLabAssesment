using BandLabAssesment.Domain;
using BandLabAssesment.Models;
using Microsoft.AspNetCore.Http;

namespace BandLabAssesment.Mappers;

public static class PostMapper
{
    public static PostDto MapToPost(this HttpRequest req, string originalImageUrl, string resizedImageUrl)
    {
        return new PostDto(
            req.Form.TryGetValue("caption", out var caption) ? caption : default,
            originalImageUrl,
            resizedImageUrl,
            req.Form.TryGetValue("creatorId", out var creatorId) ? creatorId : default,
            req.Form.TryGetValue("creator", out var creator) ? creator : default
            );
    }

    public static Post MapToDomain(this PostDto post)
    {
        return new Post(post.UserId, post.Creator, post.OriginalImageUrl, post.ResizedImageUrl)
        {
            Caption = post.Caption
        };
    }
}