namespace TwitterAPI.Analyzer.Common.Configuration;

public class TwitterClientConfiguration
{
    public const string Section = nameof(TwitterClientConfiguration);
    public string ConsumerKey { get; init; } = "?";
    public string ConsumerSecret { get; init; } = "?";
    public string BearerToken { get; init; } = "?";
}