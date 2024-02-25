using BandLabAssesment.Configuration;
using BandLabAssesment.Domain;
using Microsoft.Azure.Cosmos;

namespace BandLabAssesment.Persistence;

public class PostRepository : BaseRepository<Post>
{
    public PostRepository(CosmosClient cosmosClient, CosmosDb settings)
        : base(cosmosClient, settings, "posts")
    {
    }
}