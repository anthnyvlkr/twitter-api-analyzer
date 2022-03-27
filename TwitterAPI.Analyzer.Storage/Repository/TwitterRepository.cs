using Serilog;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Storage.FakeDb;

namespace TwitterAPI.Analyzer.Storage.Repository;

public class TwitterRepository : ITwitterRepository
{
    private readonly IFakeDb _fakeDb;
    private readonly ILogger _logger;
    
    public TwitterRepository(
        IFakeDb fakeDb,
        ILogger logger)
    {
        _fakeDb = fakeDb ?? throw new ArgumentNullException(nameof(fakeDb));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SaveTweetAsync(TweetV2 tweet)
    {
        _fakeDb.SaveTweet(tweet);
    }

    public void IncrementTweetCount()
    {
        _fakeDb.SaveCount();
    }
    
    public long GetTweetCount()
    {
        return _fakeDb.GetTweetCount();
    }

    public string[] GetTweetIds()
    {
        return _fakeDb.GetTweetIds();
    }
}