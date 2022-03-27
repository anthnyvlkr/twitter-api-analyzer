using Microsoft.Extensions.Hosting;
using Serilog;
using Tweetinvi.Events.V2;
using Tweetinvi.Streaming.V2;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Factory;
using TwitterAPI.Analyzer.Common.Services;

namespace TwitterAPI.Analyzer.Common.BackgroundServices;

public class TwitterStreamWorker: BackgroundService
{
    private readonly ITwitterClientFactory _twitterClientFactory;
    private readonly ITweetCalculationService _tweetCalculationService;
    private readonly ILogger _logger;
    
    private ISampleStreamV2? _sampleStream;

    public TwitterStreamWorker(
        ITwitterClientFactory twitterClientFactory, 
        ITweetCalculationService tweetCalculationService,
        ILogger logger)
    {
        _twitterClientFactory = twitterClientFactory ?? throw new ArgumentNullException(nameof(twitterClientFactory));
        _tweetCalculationService = tweetCalculationService ?? throw new ArgumentNullException(nameof(tweetCalculationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sampleStream = null;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Verbose("{Class}.{Method}: Beginning execution", 
            nameof(TwitterStreamWorker), nameof(ExecuteAsync));

        try
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _logger.Warning("{Class}.{Method}: Cancellation requested",
                    nameof(TwitterStreamWorker), nameof(ExecuteAsync));

                return;
            }

            _logger.Information("{Class}.{Method}: Creating Twitter Client...",
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));

            var twitterClient = _twitterClientFactory.CreateTwitterClient();
            _sampleStream = twitterClient.StreamsV2.CreateSampleStream();
            _sampleStream.TweetReceived += TweetReceivedEventHandler;

            _logger.Information("{Class}.{Method}: Starting Twitter StreamV2...",
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));

            // todo: this will throw an exception after stopAsync is called
            // I give up trying to prevent it from happening...
            // the ISampleStreamV2 seems to be much more limited than ISampleStream
            await _sampleStream.StartAsync();
        }
        catch (TwitterClientFactoryException e)
        {
            _logger.Error(e, "{Class}.{Method}: Unable to create TwitterClient",
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));            
        }
        catch (TweetCalculationServiceException e)
        {
            _logger.Error(e, "{Class}.{Method}: Failed to calculate statistics",
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));            
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: An error occurred while streaming tweets",
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution complete", 
                nameof(TwitterStreamWorker), nameof(ExecuteAsync));
            
            _sampleStream?.StopStream();
        }
    }
    
    private void TweetReceivedEventHandler(
        object? sender, 
        TweetV2ReceivedEventArgs tweetReceivedEventArgs)
    {
        _logger.Verbose("{Class}.{Method}: Event handler triggered", 
            nameof(TwitterStreamWorker), nameof(TweetReceivedEventHandler));
        
        // start timer if not running
        if (!_tweetCalculationService.IsTimerRunning) _tweetCalculationService.StartTimer();
        
        // create task for each event
        // short lived and should be recovered and reused by the thread pool
        Task.Run(() => _tweetCalculationService.ReceivedTweetEvent());
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Warning("{Class}.{Method}: Stopping",
            nameof(TwitterStreamWorker), nameof(StopAsync));
        
        _tweetCalculationService.StopTimer();

        // todo: this, for some unknown reason, causes an exception on 58 if host.StopAsync is called
        // ISampleStreamV2 is limited compared to ISampleStream
        // Genuinely have no idea how else this is supposed to be stopped...
        _sampleStream?.StopStream();

        await base.StopAsync(cancellationToken);
    }
}