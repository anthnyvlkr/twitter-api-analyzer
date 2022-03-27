using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.FakeDb;

public interface IFakeDb
{
    void SaveCount();
    void SaveTweetId(TweetV2 tweet);
    public void SaveHashtag(HashtagV2[] hashtags);
    long GetTweetCount();
    string[] GetTweetIds();
    Dictionary<string, long> GetHashtags();
}