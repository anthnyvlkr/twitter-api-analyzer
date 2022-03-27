using System.Diagnostics;
using Serilog;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Storage.Repository;

namespace TwitterAPI.Analyzer.Common.Services;

public class TweetCalculationService : ITweetCalculationService
{
    private readonly ITwitterRepository _twitterRepository;
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;
    
    
    public TweetCalculationService(
        ITwitterRepository twitterRepository, 
        ILogger logger)
    {
        _twitterRepository = twitterRepository ?? throw new ArgumentNullException(nameof(twitterRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stopwatch = new Stopwatch();
    }

    public bool IsTimerRunning => _stopwatch.IsRunning;
    
    public void StartTimer() => _stopwatch.Start();
    
    public void StopTimer() => _stopwatch.Stop();
    
    public void ReceivedTweetEvent(TweetV2  tweet)
    {
        Task.Run(() =>
        {
            _twitterRepository.IncrementTweetCount();
            _twitterRepository.SaveTweetAsync(tweet);
        });
    }
    
    private static double CalculateTweetsPerMinute(TimeSpan timeSpan, long count) => count / timeSpan.TotalMinutes;
    private static double CalculateTweetsPerSecond(TimeSpan timeSpan, long count) => count / timeSpan.TotalSeconds;
    
    public TweetStatistics GetTweetStreamStatistics()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));

        try
        {
            // get count and elapsed at this point to avoid
            // count increasing or time continuing
            var count = _twitterRepository.GetTweetCount();
            var elapsedTimeSpan = _stopwatch.Elapsed;

            var tweetsPerMinute = CalculateTweetsPerMinute(elapsedTimeSpan, count);
            var tweetsPerSecond = CalculateTweetsPerSecond(elapsedTimeSpan, count);

            return new TweetStatistics
            {
                TotalCount = count,
                PerMinute = tweetsPerMinute,
                PerSecond = tweetsPerSecond,
                RunTime = _stopwatch.Elapsed
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
}