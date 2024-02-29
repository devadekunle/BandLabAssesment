using BandLabAssesment.Extensions;

namespace BandLabAssesment.UnitTests;

public class StringExtensionTests
{
    [Fact]
    public void ResolvePartitionKeyFromId_ReturnsPartitonKey()
    {
        var idSegment = Ulid.NewUlid();
        var partitionKeySegment = Ulid.NewUlid();
        var id = $"{idSegment}-{partitionKeySegment}";

        var result = id.ResolvePartitionKeyFromId();
        Assert.Equal(result, partitionKeySegment.ToString());
    }

    [Theory, InlineData("ThisIsAnInvalidId")]
    [InlineData("a77e6e86-71dc-4fe9-af3c-ebd51b65ba24")]
    [InlineData("01HQRMVK211Y8KWYQ40MWWY1X4")]
    public void ResolvePartitionKeyFromId_ThrowsForInvalidId(string id)
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var result = id.ResolvePartitionKeyFromId();
        });
    }
}