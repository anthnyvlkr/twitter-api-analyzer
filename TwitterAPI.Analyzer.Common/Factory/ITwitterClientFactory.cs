using Tweetinvi;

namespace TwitterAPI.Analyzer.Common.Factory;

public interface ITwitterClientFactory
{
    ITwitterClient CreateTwitterClient();
}