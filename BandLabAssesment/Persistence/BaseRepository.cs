using BandLabAssesment.Configuration;
using BandLabAssesment.Domain;
using BandLabAssesment.Extensions;
using BandLabAssesment.Repository;
using Microsoft.Azure.Cosmos;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Persistence;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly Container _container;

    public BaseRepository(CosmosClient cosmosClient, CosmosDb config, string containerName)
        => _container = cosmosClient.GetContainer(config.DatabaseName, containerName);

    public Task UpsertAsync(T item, CancellationToken token)
        => _container.UpsertItemAsync(item, new(item.Id.ResolvePartitionKeyFromId()), cancellationToken: token);

    public async Task<T> GetById(string id, CancellationToken token)
        => await _container.ReadItemAsync<T>(id, new(id.ResolvePartitionKeyFromId()), cancellationToken: token);
}