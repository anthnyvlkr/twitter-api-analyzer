using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.Repository;

public interface ITwitterRepository
{
   void SaveTweet(TweetV2 tweet);
   void IncrementTweetCount();
   long GetTweetCount();
   string[] GetTweetIds();
   Dictionary<string, long> GetHashtags();
}