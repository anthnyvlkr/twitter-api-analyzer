using System.Collections.Concurrent;
using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.FakeDb;

public class FakeDb : IFakeDb
{
    private static readonly ConcurrentDictionary<string, TweetV2> TweetStorage;
    private static readonly ConcurrentBag<string> TweetIds;
    private static long _tweetCount;

    static FakeDb()
    {
        TweetStorage = new ConcurrentDictionary<string, TweetV2>();
        TweetIds = new ConcurrentBag<string>();
    }

    public void SaveCount()
    {
        Interlocked.Increment(ref _tweetCount);
    }

    public void SaveTweet(TweetV2 tweet)
    {
        TweetIds.Add(tweet.Id);
        TweetStorage.TryAdd(tweet.Id, tweet);
    }

    public TweetV2? GetTweetById(string tweetId)
    {
        if (TweetStorage.TryGetValue(tweetId, out var value))
            return value;

        return default;
    }

    public long GetTweetCount() => _tweetCount;

    public string[] GetTweetIds() => TweetIds.ToArray();
}