using Serilog;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Storage.Exceptions;
using TwitterAPI.Analyzer.Storage.Repository;

namespace TwitterAPI.Analyzer.Common.Services;

public class TweetCalculationService : ITweetCalculationService
{
    private readonly ITwitterRepository _twitterRepository;
    private readonly IStopwatchService _stopwatchService;
    private readonly ILogger _logger;

    public TweetCalculationService(
        ITwitterRepository twitterRepository, 
        IStopwatchService stopwatchService,
        ILogger logger)
    {
        _twitterRepository = twitterRepository ?? throw new ArgumentNullException(nameof(twitterRepository));
        _stopwatchService = stopwatchService ?? throw new ArgumentNullException(nameof(stopwatchService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool IsTimerRunning => _stopwatchService.IsRunning;
    
    public void StartTimer() => _stopwatchService.StartTimer();
    
    public void StopTimer() => _stopwatchService.StopTimer();
    
    public void ReceivedTweetEvent(TweetV2  tweet)
    {
        try
        {
            Task.Run(() =>
            {
                _twitterRepository.IncrementTweetCount();
                _twitterRepository.SaveTweet(tweet);
            });
        }
        catch (TwitterRepositoryException e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while saving tweet",
                nameof(TweetCalculationService), nameof(ReceivedTweetEvent));
        }
    }
    
    private static double CalculateTweetsPerMinute(TimeSpan timeSpan, long count) => count / timeSpan.TotalMinutes;
    private static double CalculateTweetsPerSecond(TimeSpan timeSpan, long count) => count / timeSpan.TotalSeconds;

    public TweetStatistics GetTweetStreamStatistics()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));

        try
        {
            // get count and elapsed at this point to avoid count increasing or time continuing
            var count = _twitterRepository.GetTweetCount();
            var elapsedTimeSpan = _stopwatchService.Elapsed;

            var tweetsPerMinute = CalculateTweetsPerMinute(elapsedTimeSpan, count);
            var tweetsPerSecond = CalculateTweetsPerSecond(elapsedTimeSpan, count);

            return new TweetStatistics
            {
                TotalCount = count,
                PerMinute = tweetsPerMinute,
                PerSecond = tweetsPerSecond,
                RunTime = elapsedTimeSpan
            };
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while calculating statistics",
                nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));

            throw new TweetCalculationServiceException("An error occurred while calculating statistics", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));
        }
    }

    public IEnumerable<HashtagStatistics>? GetHashtagStatistics()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TweetCalculationService), nameof(GetHashtagStatistics));

        try
        {
            return _twitterRepository
                .GetHashtags()
                .Select(c => new HashtagStatistics
                {
                    HashTag = c.Key,
                    Count = c.Value
                })
                .OrderByDescending(c => c.Count);
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while retrieving hashtag statistics",
                nameof(TweetCalculationService), nameof(GetHashtagStatistics));

            return null;
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TweetCalculationService), nameof(GetHashtagStatistics));
        }
    }
}