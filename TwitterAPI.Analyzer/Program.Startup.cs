using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TwitterAPI.Analyzer.BackgroundServices;
using TwitterAPI.Analyzer.Common.BackgroundServices;
using TwitterAPI.Analyzer.Common.Configuration;
using TwitterAPI.Analyzer.Common.Factory;
using TwitterAPI.Analyzer.Common.Services;
using TwitterAPI.Analyzer.Configuration;

namespace TwitterAPI.Analyzer;

public partial class Program
{
    private static IConfigurationRoot? BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();
    }
    
    private static IHostBuilder CreateHostBuilder(IConfiguration? configuration)
    {
        return Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                hostContext.Configuration = configuration;

                // Background Services
                services.AddHostedService<ConsoleUpdateWorker>()
                    .AddLogging(builder => builder.AddSerilog(Log.Logger));
                
                services.AddHostedService<TwitterStreamWorker>()
                    .AddLogging(builder => builder.AddSerilog(Log.Logger));

                // Service dependencies
                services.AddSingleton<ITweetCalculationService, TweetCalculationService>();
                services.AddSingleton<ITwitterClientFactory, TwitterClientFactory>();
                services.AddSingleton(Log.Logger);
                
                // options
                services.Configure<TwitterClientConfiguration>(
                    hostContext.Configuration?.GetSection(TwitterClientConfiguration.Section));
                services.Configure<ConsoleUpdateWorkerConfiguration>(
                    hostContext.Configuration?.GetSection(ConsoleUpdateWorkerConfiguration.Section));

            });
    }
        
}