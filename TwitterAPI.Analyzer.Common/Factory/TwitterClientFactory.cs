using Microsoft.Extensions.Options;
using Serilog;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterAPI.Analyzer.Common.Configuration;
using TwitterAPI.Analyzer.Common.Exceptions;

namespace TwitterAPI.Analyzer.Common.Factory;

public class TwitterClientFactory : ITwitterClientFactory
{
    private readonly TwitterClientConfiguration _twitterClientConfiguration;
    private readonly ILogger _logger;

    public TwitterClientFactory(
        IOptions<TwitterClientConfiguration> twitterClientConfiguration,
        ILogger logger)
    {
        _twitterClientConfiguration = twitterClientConfiguration.Value ?? 
                                      throw new ArgumentNullException(nameof(twitterClientConfiguration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public ITwitterClient CreateTwitterClient()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterClientFactory), nameof(CreateTwitterClient));
        
        try
        {
            _logger.Information("{Class}.{Method}: Creating TwitterClient",
                nameof(TwitterClientFactory), nameof(CreateTwitterClient));

            return new TwitterClient(TwitterCredentials);
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while creating TwitterClient",
                nameof(TwitterClientFactory), nameof(CreateTwitterClient));
            
            throw new TwitterClientFactoryException("An error occurred while creating client", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterClientFactory), nameof(CreateTwitterClient));
        }
    }

    private ITwitterCredentials TwitterCredentials => new TwitterCredentials(
        _twitterClientConfiguration.ConsumerKey,
        _twitterClientConfiguration.ConsumerSecret,
        _twitterClientConfiguration.BearerToken
    );
}