using BandLabAssesment.Configuration;
using BandLabAssesment.Domain;
using BandLabAssesment.Repository;
using Microsoft.Azure.Cosmos;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Persistence;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly Container _container;

    public BaseRepository(CosmosClient cosmosClient, CosmosDb config, string containerName)
    {
        _container = cosmosClient.GetContainer(config.DatabaseName, containerName);
    }

    public Task UpsertAsync(T item, string partitionKeyValue, CancellationToken token)
    {
        return _container.UpsertItemAsync<T>(item, new PartitionKey(partitionKeyValue), cancellationToken: token);
    }
}