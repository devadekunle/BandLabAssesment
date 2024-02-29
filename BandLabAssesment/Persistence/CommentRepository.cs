using BandLabAssesment.Configuration;
using BandLabAssesment.Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BandLabAssesment.Persistence;

public class CommentRepository : BaseRepository<Comment>
{
    public CommentRepository(CosmosClient cosmosClient, CosmosDb settings)
        : base(cosmosClient, settings, CosmosDb.CommentsContainer)
    {
    }

    public async Task<Comment> GetLatestCommentSince(DateTime createdAt)
    {
        var query = _container.GetItemLinqQueryable<Comment>(requestOptions: new QueryRequestOptions { MaxItemCount = 1 })
            .Where(e => e.CreatedAt > createdAt);

        using var feedIterator = query.ToFeedIterator<Comment>();
        while (feedIterator.HasMoreResults)
        {
            var item = await feedIterator.ReadNextAsync();
            return item.Resource.FirstOrDefault();
        }

        return default;
    }
}