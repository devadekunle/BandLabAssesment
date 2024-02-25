namespace BandLabAssesment;

public static class Constants
{
    public static class Functions
    {
        public const string CreatePostV1 = "api-create-post-v1";
        public const string CreatePostCommentV1 = "api-create-post-comment-v1";
    }

    public static class ApiRoutes
    {
        public const string CreatePostV1 = "v1/posts";
        public const string CreatePostCommentV1 = "v1/posts/{postId}/comments";
    }

    public static string[] AllowedFileTypes = { "image/jpg", "image/png", "image/bnp", "image/jpeg" };

    public const long MaxFileSize = 104_857_600; //100MB

    public static class BlobStorage
    {
        public const string OriginalImageFolder = "original";
        public const string ResizedImageFolder = "resized";
        public const long InitialTransferSize = 94371840;
    }
}