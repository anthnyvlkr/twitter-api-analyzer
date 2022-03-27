using System.Collections.Concurrent;
using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.FakeDb;

public class FakeDb : IFakeDb
{
    private static readonly ConcurrentDictionary<string, long> TweetHashtagStorage;
    private static readonly ConcurrentBag<string> TweetIds;
    private static long _tweetCount;

    static FakeDb()
    {
        TweetHashtagStorage = new ConcurrentDictionary<string, long>();
        TweetIds = new ConcurrentBag<string>();
    }

    public void SaveCount()
    {
        Interlocked.Increment(ref _tweetCount);
    }

    public void SaveTweetId(TweetV2 tweet)
    {
        TweetIds.Add(tweet.Id);
    }

    public void SaveHashtag(HashtagV2[] hashtags)
    {
        foreach (var hashtag in hashtags)
        {
            TweetHashtagStorage.AddOrUpdate(hashtag.Tag, 1, 
                (key, oldValue) => oldValue + 1);
        }
    }

    public long GetTweetCount() => _tweetCount;

    public string[] GetTweetIds() => TweetIds.ToArray();
    public Dictionary<string, long> GetHashtags() => 
        TweetHashtagStorage
            .ToDictionary(
                k => k.Key, 
                v => v.Value, 
                TweetHashtagStorage.Comparer);
}