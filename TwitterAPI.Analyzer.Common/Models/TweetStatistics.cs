namespace TwitterAPI.Analyzer.Common.Models;

public class TweetStatistics
{
    public long TotalCount { get; init; }
    public double PerMinute { get; init; }
    public double PerSecond { get; init; }

    public TimeSpan RunTime { get; init; }
    
    public override string ToString()
    {
        return $"| TotalCount: {TotalCount} " +
               $"| PerSecond: {PerSecond:F} " + 
               $"| PerMinute: {PerMinute:F}" +
               $"| RunTime: {RunTime} |";
    }
}