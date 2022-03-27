using System.Diagnostics;
using Serilog;
using TwitterAPI.Analyzer.Common.Models;

namespace TwitterAPI.Analyzer.Common.Services;

public class TweetCalculationService : ITweetCalculationService
{
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch;
    private int _tweetCount;

    public TweetCalculationService(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _stopwatch = new Stopwatch();
    }

    public bool IsTimerRunning => _stopwatch.IsRunning;
    
    public void StartTimer() => _stopwatch.Start();
    
    public void StopTimer() => _stopwatch.Stop();

    public void ReceivedTweetEvent() => Interlocked.Increment(ref _tweetCount);

    private static double CalculateTweetsPerMinute(TimeSpan timeSpan, int count) => count / timeSpan.TotalMinutes;
    private static double CalculateTweetsPerSecond(TimeSpan timeSpan, int count) => count / timeSpan.TotalSeconds;
    
    public TweetStatistics? GetTweetStreamStatistics()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));

        try
        {
            // get count and elapsed at this point to avoid
            // count increasing or time continuing
            var count = _tweetCount;
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
            // todo: custom exception and log
            Console.WriteLine(e);
            return default;
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TweetCalculationService), nameof(GetTweetStreamStatistics));
        }
    }
}