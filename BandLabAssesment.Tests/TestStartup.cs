using BandLabAssesment.Functions.V1;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BandLabAssesment.Tests;

public class TestStartup : Startup
{
    protected override IConfigurationRoot GetConfigurationRoot(IFunctionsHostBuilder functionsHostBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("tests.settings.json", true)
            .AddEnvironmentVariables()
            .Build();

        return configuration;
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        base.Configure(builder);
        builder.Services.AddTransient<ILogger, InMemoryLogger>();

        builder.Services.AddTransient<AddPostComment>();
        builder.Services.AddTransient<CreatePost>();
        builder.Services.AddTransient<DeleteComment>();
    }
}