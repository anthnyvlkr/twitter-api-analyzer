using Tweetinvi.Models.V2;

namespace TwitterAPI.Analyzer.Storage.Repository;

public interface ITwitterRepository
{
   void SaveTweetAsync(TweetV2 tweet);
   void IncrementTweetCount();
   long GetTweetCount();
   string[] GetTweetIds();
}