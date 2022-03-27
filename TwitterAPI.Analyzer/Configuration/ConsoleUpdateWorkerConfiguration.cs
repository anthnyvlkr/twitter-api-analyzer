namespace TwitterAPI.Analyzer.Configuration;

public class ConsoleUpdateWorkerConfiguration
{
    public const string Section = nameof(ConsoleUpdateWorkerConfiguration);
    public double UpdateIntervalMilliseconds { get; init; }
}