using TwitterAPI.Analyzer.Common.Models;

namespace TwitterAPI.Analyzer.Common.Services;

public interface ITweetCalculationService
{
    public bool IsTimerRunning { get; }
    public void StartTimer();
    public void StopTimer();
    void ReceivedTweetEvent();
    public TweetStatistics? GetTweetStreamStatistics();
}