using Serilog;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.Storage.Exceptions;
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
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterRepository), nameof(SaveTweet));
        
        try
        {
            _fakeDb.SaveTweetId(tweet);
        
            if (tweet.Entities.Hashtags != null && tweet.Entities.Hashtags.Any()) 
                _fakeDb.SaveHashtag(tweet.Entities.Hashtags);
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while saving tweet",
                nameof(TwitterRepository), nameof(SaveTweet));
            
            throw new TwitterRepositoryException("An error occurred while saving a tweet", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterRepository), nameof(SaveTweet));
        }
    }

    public void IncrementTweetCount()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterRepository), nameof(IncrementTweetCount));

        try
        {
            _fakeDb.SaveCount();
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while updating tweet count",
                nameof(TwitterRepository), nameof(IncrementTweetCount));
            
            throw new TwitterRepositoryException("An error occurred while updating tweet count", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterRepository), nameof(IncrementTweetCount));
        }
    }
    
    public long GetTweetCount()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterRepository), nameof(GetTweetCount));

        try
        {
            return _fakeDb.GetTweetCount();
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while getting tweet count",
                nameof(TwitterRepository), nameof(GetTweetCount));
            
            throw new TwitterRepositoryException("An error occurred while getting tweet count", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterRepository), nameof(GetTweetCount));
        }
    }

    public string[] GetTweetIds()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterRepository), nameof(GetTweetIds));

        try
        {
            return _fakeDb.GetTweetIds();
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while getting tweetIds ",
                nameof(TwitterRepository), nameof(GetTweetIds));
            
            throw new TwitterRepositoryException("An error occurred while getting tweetIds", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterRepository), nameof(GetTweetIds));
        }
    }
    
    public Dictionary<string, long> GetHashtags()
    {
        _logger.Verbose("{Class}.{Method}: Beginning Execution",
            nameof(TwitterRepository), nameof(GetHashtags));

        try
        {
            return _fakeDb.GetHashtags();
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Exception occurred while getting hashtags ",
                nameof(TwitterRepository), nameof(GetHashtags));
            
            throw new TwitterRepositoryException("An error occurred while getting hashtags", e);
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution Complete",
                nameof(TwitterRepository), nameof(GetHashtags));
        }
    }
}