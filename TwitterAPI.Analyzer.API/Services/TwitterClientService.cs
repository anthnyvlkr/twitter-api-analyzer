using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Common.Factory;
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
    
    public async Task<TweetV2Response?> GetRandomTweet()
    {
        try
        {
            var randomTweetIds = _twitterRepository.GetTweetIds();
            var randomIndex = new Random().Next(randomTweetIds.Length);
            var randomId = randomTweetIds[randomIndex];
            
            var twitterClient = _twitterClientFactory.CreateTwitterClient();
            return await twitterClient.TweetsV2.GetTweetAsync(randomId);
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while retrieving current trends",
                nameof(TwitterClientService), nameof(GetRandomTweet));

            return null;
        }
    }
}