using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using TwitterAPI.Analyzer.Common.BackgroundServices;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Services;
using TwitterAPI.Analyzer.Configuration;

namespace TwitterAPI.Analyzer.BackgroundServices;

public class ConsoleUpdateWorker: BackgroundService
{
    private readonly ITweetCalculationService _tweetCalculationService;
    private readonly ILogger _logger;
    private readonly System.Timers.Timer _timer;
    
    public ConsoleUpdateWorker(
        ITweetCalculationService tweetCalculationService,
        IOptions<ConsoleUpdateWorkerConfiguration> consoleUpdateWorkerConfiguration,
        ILogger logger)
    {
        _tweetCalculationService = tweetCalculationService ?? throw new ArgumentNullException(nameof(tweetCalculationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _timer = new System.Timers.Timer(consoleUpdateWorkerConfiguration.Value.UpdateIntervalMilliseconds);
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Verbose("{Class}.{Method}: Beginning execution", 
            nameof(ConsoleUpdateWorker), nameof(ExecuteAsync));

        try
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _logger.Warning("{Class}.{Method}: Cancellation requested",
                    nameof(ConsoleUpdateWorker), nameof(ExecuteAsync));

                return Task.CompletedTask;
            }
        
            _logger.Information("{Class}.{Method}: Starting Timer...", 
                nameof(ConsoleUpdateWorker), nameof(ExecuteAsync));
            
            _timer.Elapsed += UpdateConsoleEventHandler;
            _timer.Start();

            return Task.CompletedTask;
        }
        catch (TweetCalculationServiceException e)
        {
            _logger.Error(e, "{Class}.{Method}: Failed to calculate statistics",
                nameof(ConsoleUpdateWorker), nameof(ExecuteAsync));            
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Class}.{Method}: Error occurred while streaming tweets",
                nameof(ConsoleUpdateWorker), nameof(ExecuteTask));
            
            _timer.Stop();
            _timer.Dispose();
        }
        finally
        {
            _logger.Verbose("{Class}.{Method}: Execution complete", 
                nameof(ConsoleUpdateWorker), nameof(ExecuteAsync));
        }
        
        return Task.CompletedTask;
    }
    
    private void UpdateConsoleEventHandler(object? source, System.Timers.ElapsedEventArgs e)
    {
        _logger.Verbose("{Class}.{Method}: Event handler triggered", 
            nameof(TwitterStreamWorker), nameof(UpdateConsoleEventHandler));

        Console.WriteLine("Tweet Frequency: {0} ", _tweetCalculationService.GetTweetStreamStatistics());
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Warning("{Class}.{Method}: Stopping",
            nameof(ConsoleUpdateWorker), nameof(StopAsync));
    
        _timer.Stop();
        _timer.Dispose();
        return base.StopAsync(cancellationToken);
    }
}