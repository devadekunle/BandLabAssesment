using Azure.Storage.Blobs;
using BandLabAssesment.Configuration;
using BandLabAssesment.Persistence;
using BandLabAssesment.Repository;
using BandLabAssesment.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(BandLabAssesment.Startup))]

namespace BandLabAssesment;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        ConfigureBlobStorage(builder);
        ConfigureCosmosDb(builder);
        builder.Services.AddScoped<ImageUploader>();
        builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<PostRepository>();
        builder.Services.AddScoped<CommentRepository>();
        builder.Services.AddScoped<PostsService>();
    }

    private void ConfigureCosmosDb(IFunctionsHostBuilder builder)
    {
        var cosmosConfig = builder.GetContext().Configuration.GetSection(nameof(CosmosDb)).Get<CosmosDb>();
        var cosmosClient = new CosmosClientBuilder(cosmosConfig.ConnectionString)
            .WithContentResponseOnWrite(false)
            .WithSerializerOptions(new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase })
            .Build();

        builder.Services.AddSingleton(cosmosConfig);

        builder.Services.AddSingleton(cosmosClient);
    }

    private static void ConfigureBlobStorage(IFunctionsHostBuilder builder)
    {
        var blobStorageSettings = builder.GetContext().Configuration.GetSection(nameof(BlobStorage)).Get<BlobStorage>();
        ArgumentNullException.ThrowIfNull(blobStorageSettings);

        var blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);
        builder.Services.AddSingleton(blobServiceClient.GetBlobContainerClient(blobStorageSettings.Container));
    }
}