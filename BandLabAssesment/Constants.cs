namespace BandLabAssesment;

public static class Constants
{
    public static class Functions
    {
        public const string CreatePostV1 = "api-create-post-v1";
        public const string CreatePostCommentV1 = "api-create-post-comment-v1";
        public const string DeleteCommentV1 = "api-delete-post-comment-v1";
        public const string UpdatePostWithComments = "changefeed-update-post-with-comments";
    }

    public static class ApiRoutes
    {
        public const string CreatePostV1 = "v1/posts";
        public const string CreatePostCommentV1 = "v1/posts/{postId}/comments";
        public const string DeletePostCommentV1 = "v1/posts/{postId}/comments/{commentId}";
    }

    public static string[] AllowedFileTypes = { "image/jpg", "image/png", "image/bnp", "image/jpeg" };

    public const long MaxFileSize = 104_857_600; //100MB

    public static class BlobStorage
    {
        public const string OriginalImageFolder = "original";
        public const string ResizedImageFolder = "resized";
        public const long InitialTransferSize = 94371840;
    }

    public static class ErrorMessages
    {
        public const string CreatorIdNotProvided = "CreatorId is required";
        public const string CreatorNotProvided = "Creator is required";
        public const string ImageNotProvided = "A post must have one image";
        public const string FileTypeNotSupported = "Image type not supported";
        public const string ImageTooLarge = "Image too large";
    }
}