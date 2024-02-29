using Azure.Storage.Blobs;
using BandLabAssesment.Configuration;
using BandLabAssesment.Persistence;
using BandLabAssesment.Repository;
using BandLabAssesment.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

[assembly: FunctionsStartup(typeof(BandLabAssesment.Startup))]

namespace BandLabAssesment;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = GetConfigurationRoot(builder);
        ConfigureBlobStorage(configuration, builder);
        ConfigureCosmosDb(configuration, builder);
        ConfigureApplicationServices(builder);
    }

    protected virtual IConfigurationRoot GetConfigurationRoot(IFunctionsHostBuilder functionsHostBuilder)
    {
        var services = functionsHostBuilder.Services;

        var executionContextOptions = services
            .BuildServiceProvider()
            .GetService<IOptions<ExecutionContextOptions>>()
            .Value;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(executionContextOptions.AppDirectory)
            .AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables()
            .Build();

        return configuration;
    }

    private static void ConfigureApplicationServices(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<ImageUploader>();
        builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<PostRepository>();
        builder.Services.AddScoped<CommentRepository>();
        builder.Services.AddScoped<PostsService>();
    }

    private static void ConfigureBlobStorage(IConfiguration configuration, IFunctionsHostBuilder builder)
    {
        var blobStorageSettings = configuration.GetSection(nameof(BlobStorage)).Get<BlobStorage>();
        ArgumentNullException.ThrowIfNull(blobStorageSettings);

        var blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);
        builder.Services.AddSingleton(blobServiceClient.GetBlobContainerClient(blobStorageSettings.Container));
    }

    private static void ConfigureCosmosDb(IConfiguration configuration, IFunctionsHostBuilder builder)
    {
        var cosmosConfig = configuration.GetSection(nameof(CosmosDb)).Get<CosmosDb>();
        var cosmosClient = new CosmosClientBuilder(cosmosConfig.ConnectionString)
            .WithContentResponseOnWrite(false)
            .WithSerializerOptions(new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase })
            .Build();

        builder.Services.AddSingleton(cosmosConfig);

        builder.Services.AddSingleton(cosmosClient);
    }
}