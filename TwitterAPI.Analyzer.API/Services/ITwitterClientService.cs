using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.API.Services;

public interface ITwitterClientService
{
    Task<TweetV2Response?> GetRandomTweet();
}