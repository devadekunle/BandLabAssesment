namespace BandLabAssesment.Models;

public record PostDto(
    string Caption,
    string OriginalImageUrl,
    string ResizedImageUrl,
    string UserId,
    string Creator
    );