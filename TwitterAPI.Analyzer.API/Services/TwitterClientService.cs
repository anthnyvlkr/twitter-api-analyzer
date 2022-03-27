using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Factory;
using TwitterAPI.Analyzer.Storage.Exceptions;
using TwitterAPI.Analyzer.Storage.Repository;
using ILogger = Serilog.ILogger;

namespace TwitterAPI.Analyzer.API.Services;

public class TwitterClientService : ITwitterClientService
{
    private readonly ITwitterClientFactory _twitterClientFactory;
    private readonly ITwitterRepository _twitterRepository;
    private readonly ILogger _logger;

    public TwitterClientService(
        ITwitterClientFactory twitterClientFactory,
        ITwitterRepository twitterRepository,
        ILogger logger)
    {
        _twitterClientFactory = twitterClientFactory ?? throw new ArgumentNullException(nameof(twitterClientFactory));
        _twitterRepository = twitterRepository ?? throw new ArgumentNullException(nameof(twitterRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string[]? GetRecentTweetIds()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterClientService), nameof(GetRecentTweetIds));
        
        try
        {
            return _twitterRepository.GetTweetIds();
        }
        catch (TwitterRepositoryException e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while getting recent tweetIds",
                nameof(TwitterClientService), nameof(GetRecentTweetIds));

            return null;
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Complete",
                nameof(TwitterClientService), nameof(GetRecentTweetIds));
        }
    }

    public async Task<TweetV2Response?> GetTweetByIdAsync(string id)
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterClientService), nameof(GetTweetByIdAsync));
        
        try
        {
            var twitterClient = _twitterClientFactory.CreateTwitterClient();
            return await twitterClient.TweetsV2.GetTweetAsync(id);
        }
        catch (TwitterClientFactoryException e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while creating TwitterClient",
                nameof(TwitterClientService), nameof(GetTweetByIdAsync));

            return null;
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while retrieving tweet by Id {id}",
                nameof(TwitterClientService), nameof(GetTweetByIdAsync), id);

            return null;
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Complete",
                nameof(TwitterClientService), nameof(GetTweetByIdAsync));
        }
    } 
    
    public async Task<TweetV2Response?> GetRandomTweet()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterClientService), nameof(GetRandomTweet));

        try
        {
            var randomTweetIds = _twitterRepository.GetTweetIds();
            var randomIndex = new Random().Next(randomTweetIds.Length);
            var randomId = randomTweetIds[randomIndex];
            
            var twitterClient = _twitterClientFactory.CreateTwitterClient();
            return await twitterClient.TweetsV2.GetTweetAsync(randomId);
        }
        catch (TwitterRepositoryException e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while getting a random tweet",
                nameof(TwitterClientService), nameof(GetRandomTweet));

            return null;
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while retrieving random tweet",
                nameof(TwitterClientService), nameof(GetRandomTweet));

            return null;
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Complete",
                nameof(TwitterClientService), nameof(GetRandomTweet));
        }
    }
}