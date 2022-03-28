namespace TwitterAPI.Analyzer.Common.Services;

public interface IStopwatchService
{
    void StartTimer();
    void StopTimer();
    bool IsRunning { get; }
    TimeSpan Elapsed { get; }
}