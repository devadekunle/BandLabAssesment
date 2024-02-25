using BandLabAssesment.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Services;

public static class ImageResizer
{
    public static async Task<Stream> ConvertToJpg(Stream originalImage, int width, int height, bool resetStream, CancellationToken token)
    {
        using var imageStream = Image.Load(originalImage);
        imageStream.Mutate(x => x.Resize(width, height));
        var outputStream = new MemoryStream();
        await imageStream.SaveAsJpegAsync(outputStream, token);
        if (resetStream)
            outputStream.Reset();
        return outputStream;
    }
}