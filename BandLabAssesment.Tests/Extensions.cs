using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

public static class Helpers
{
    public static IFormFile CreateFormFile(string keyname, string filePath, string contentType)
    {
        var stream = File.OpenRead(filePath);
        return new FormFile(stream, 0, stream.Length, keyname, filePath)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}