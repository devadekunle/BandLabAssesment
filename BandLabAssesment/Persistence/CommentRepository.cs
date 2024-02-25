using BandLabAssesment.Configuration;
using BandLabAssesment.Domain;
using Microsoft.Azure.Cosmos;

namespace BandLabAssesment.Persistence;

public class CommentRepository : BaseRepository<Comment>
{
    public CommentRepository(CosmosClient cosmosClient, CosmosDb settings)
        : base(cosmosClient, settings, "comments")
    {
    }
}