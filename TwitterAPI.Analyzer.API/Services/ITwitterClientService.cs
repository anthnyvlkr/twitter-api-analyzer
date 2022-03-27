using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.API.Services;

public interface ITwitterClientService
{
    string[]? GetRecentTweetIds();
    Task<TweetV2Response?> GetTweetByIdAsync(string id);
    Task<TweetV2Response?> GetRandomTweet();
}