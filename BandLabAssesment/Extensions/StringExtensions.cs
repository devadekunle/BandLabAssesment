using System;

namespace BandLabAssesment.Extensions;

public static class StringExtensions
{
    public static string ResolvePartitionKeyFromId(this string Id)
    {
        var segments = Id.Split('-');

        if (segments.Length < 2 || !Ulid.TryParse(segments[1], out var partitionKey))
        {
            throw new InvalidOperationException("Partition Key could not be resolved from this Id");
        }
        return partitionKey.ToString();
    }
}