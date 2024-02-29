using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

public class MultipartFormRequestBuilder
{
    private new Dictionary<string, StringValues> Map = new();

    private IFormFile Image;

    public MultipartFormRequestBuilder SetCreator(string creator)
    {
        Map.Add(nameof(creator), creator);
        return this;
    }

    public MultipartFormRequestBuilder SetCreatorId(string creatorId)
    {
        Map.Add(nameof(creatorId), creatorId);
        return this;
    }

    public MultipartFormRequestBuilder SetCaption(string caption)
    {
        Map.Add(nameof(caption), caption);
        return this;
    }

    public MultipartFormRequestBuilder SetImage(string filePath, string contentType)
    {
        Image = Helpers.CreateFormFile("image", filePath, contentType);
        return this;
    }

    public DefaultHttpRequest Build()
    {
        var formFileCollection = new FormFileCollection();
        if (Image is not null)
        {
            formFileCollection.Add(Image);
        }

        var formCollection = new FormCollection(Map, formFileCollection);
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Method = "POST",
            ContentType = "multipart/form-data",
            Form = formCollection,
        };

        return request;
    }
}