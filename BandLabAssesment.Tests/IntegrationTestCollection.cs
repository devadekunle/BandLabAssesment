namespace BandLabAssesment.Tests;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<TestInitializer>
{
    public const string Name = nameof(IntegrationTestCollection);
}