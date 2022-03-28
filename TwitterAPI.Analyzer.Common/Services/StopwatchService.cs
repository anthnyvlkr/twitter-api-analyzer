using System.Diagnostics;

namespace TwitterAPI.Analyzer.Common.Services;

public class StopwatchService : IStopwatchService
{
    private readonly Stopwatch _stopwatch;
    
    public StopwatchService()
    {
        _stopwatch = new Stopwatch();
    }

    public void StartTimer() => _stopwatch.Start();

    public void StopTimer() => _stopwatch.Stop();

    public bool IsRunning => _stopwatch.IsRunning;

    public TimeSpan Elapsed => _stopwatch.Elapsed;
}