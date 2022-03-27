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

    public void SaveTweet(TweetV2 tweet)
    {
        try
        {
            _fakeDb.SaveTweetId(tweet);
        
            if (tweet.Entities.Hashtags != null && tweet.Entities.Hashtags.Any()) 
                _fakeDb.SaveHashtag(tweet.Entities.Hashtags);
        }
        catch (Exception e)
        {
            // todo:
            Console.WriteLine(e);
            throw;
        }
    }

    public void IncrementTweetCount()
    {
        try
        {
            _fakeDb.SaveCount();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public long GetTweetCount()
    {
        try
        {
            return _fakeDb.GetTweetCount();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string[] GetTweetIds()
    {
        try
        {
            return _fakeDb.GetTweetIds();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public Dictionary<string, long> GetHashtags()
    {
        try
        {
            return _fakeDb.GetHashtags();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}