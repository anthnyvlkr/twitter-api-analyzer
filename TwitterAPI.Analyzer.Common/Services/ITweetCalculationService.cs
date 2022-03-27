using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Common.Models;

namespace TwitterAPI.Analyzer.Common.Services;

public interface ITweetCalculationService
{
    public bool IsTimerRunning { get; }
    public void StartTimer();
    public void StopTimer();
    void ReceivedTweetEvent(TweetV2 tweet);
    TweetStatistics? GetTweetStreamStatistics();
    IEnumerable<HashtagStatistics>? GetHashtagStatistics();
}