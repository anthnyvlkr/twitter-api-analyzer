#TwitterAPI.Analyzer
This is a .net core console application that hosts two background services.

- TweetStreamBackgroundWorker
  - This service is responsible for handling the events received from the Twitter stream and handing off the results to be processed
  - This service is defined in TwitterAPI.Analyzer.Common
- ConsoleUpdateWorker
  - This service is responsible for interacting with the TweetCalculationService to retrieve the results and print them to the console

On start of the console application, both services will start. TweetStatistic updates will be printed to the console every 10 seconds, this number is configurable.

#TwitterAPI.Analyzer.API
This is an asp.net core web api application. This web api utilizes the same background service as TwitterAPI.Analyzer to connect to the Twitter sample stream.
###TweetController
Includes endpoints to GetTweetsById, GetRecentTweetIds and GetRandomTweet

###TwitterStatisticsController
Includes endpoints to get TweetStatistics, tweets per minute/second, and hashtag statistics.

#TwitterAPI.Analyzer.Common
This project is shared between other projects and includes some of the critical aspects of the available functionality.
###TweetStreamBackgroundWorker
- This service is responsible for handling the events received from the Twitter stream and handing off the results to be processed
- Used in TwitterAPI.Analyzer and TwitterAPI.Analyzer.API

#TwitterAPI.Analyzer.Storage
This is really just a fake storage source. I used concurrent collections to store data that the application uses.
###FakeDb 
Used for temporary storage during runtime
###TwitterRepository
This is the class other services will interact with to retrieve data from the "db"

#Running locally
You will need to add user secrets to both TwitterAPI.Analyzer and TwitterAPI.Analyzer.API. These secrets include the credentials needed to create a TwitterClient. You can learn more about gaining access [here]("https://developer.twitter.com/en/portal/dashboard").

#Todo:
- paging 
- real data storage
- more tests
- async things better?
- whatever else seems cool