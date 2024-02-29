using BandLabAssesment.Extensions;
using BandLabAssesment.Mappers;
using BandLabAssesment.Models;
using BandLabAssesment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BandLabAssesment.Functions.V1;

public class CreatePost
{
    private readonly ImageUploader _imageUploader;
    private readonly PostsService _postsService;

    public CreatePost(ImageUploader imageUploader, PostsService postsService)
    {
        _imageUploader = imageUploader;
        _postsService = postsService;
    }

    [FunctionName(Constants.Functions.CreatePostV1)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "POST", Route = Constants.ApiRoutes.CreatePostV1)] HttpRequest req,
        ILogger log,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = ValidateRequest(req);
            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult.ValidationError);

            var file = req.Form.Files[0];
            using var stream = file.OpenReadStream();
            var fileName = file.FileName;

            var originalImageUrl = await _imageUploader.UploadImage(stream, IsOriginalImage: true, fileName, cancellationToken);
            stream.Reset();

            using var processedImageStream = await ImageResizer.ConvertToJpg(stream, width: 600, height: 600, resetStream: true, cancellationToken);
            var resizedImageUrl = await _imageUploader.UploadImage(processedImageStream, IsOriginalImage: false, fileName, cancellationToken);

            var post = req.MapToPost(originalImageUrl, resizedImageUrl);

            var postId = await _postsService.CreatePost(post, cancellationToken);

            return new OkObjectResult(new PostCreated(postId));
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Something went wrong while processing this request");
            return new InternalServerErrorResult();
        }
    }

    private static (bool IsValid, string ValidationError) ValidateRequest(HttpRequest req)
    {
        if (!req.Form.ContainsKey("creatorId"))
            return (false, Constants.ErrorMessages.CreatorIdNotProvided);

        if (!req.Form.ContainsKey("creator"))
            return (false, Constants.ErrorMessages.CreatorNotProvided);

        if (req.Form.Files.Count < 1)
            return (false, Constants.ErrorMessages.ImageNotProvided);

        if (!Constants.AllowedFileTypes.Any(fileType => fileType.Equals(req.Form.Files[0].ContentType, StringComparison.OrdinalIgnoreCase)))
            return (false, Constants.ErrorMessages.FileTypeNotSupported);

        if (req.Form.Files[0].Length > Constants.MaxFileSize)
            return (false, "Image too large");

        return (true, default);
    }
}