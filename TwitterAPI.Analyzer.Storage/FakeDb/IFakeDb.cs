using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.FakeDb;

public interface IFakeDb
{
    void SaveCount();
    void SaveTweet(TweetV2 tweet);
    TweetV2? GetTweetById(string tweetId);
    long GetTweetCount();
    string[] GetTweetIds();
}