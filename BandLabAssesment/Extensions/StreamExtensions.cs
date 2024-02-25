using System.IO;

namespace BandLabAssesment.Extensions;

public static class StreamExtensions
{
    public static void Reset(this Stream stream) => stream.Seek(0, SeekOrigin.Begin);
}