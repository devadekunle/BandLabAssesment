using BandLabAssesment.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace BandLabAssesment.Tests;

public class TestInitializer
{
    public TestInitializer()
    {
        var startup = new TestStartup();
        var host = new HostBuilder()
              .ConfigureWebJobs(startup.Configure)
              .Build();

        ServiceProvider = host.Services;

        CosmosClient = ServiceProvider.GetRequiredService<CosmosClient>();
    }

    public IServiceProvider ServiceProvider { get; }
    private CosmosClient CosmosClient { get; }

    public Container PostsContainer
    {
        get
        {
            var config = ServiceProvider.GetRequiredService<CosmosDb>();
            return CosmosClient.GetContainer(config.DatabaseName, "posts");
        }
    }

    public Container CommentsContainer
    {
        get
        {
            var config = ServiceProvider.GetRequiredService<CosmosDb>();
            return CosmosClient.GetContainer(config.DatabaseName, "comments");
        }
    }
}