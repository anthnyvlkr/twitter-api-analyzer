using Microsoft.AspNetCore.Mvc;
using TwitterAPI.Analyzer.API.Services;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Common.Services;
using ILogger = Serilog.ILogger;

namespace TwitterAPI.Analyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TwitterStatisticsController : ControllerBase
{
    private readonly ITweetCalculationService _tweetCalculationService;
    private readonly ILogger _logger;

    public TwitterStatisticsController(
        ITweetCalculationService tweetCalculationService,
        ITwitterClientService twitterClientService,
        ILogger logger)
    {
        _tweetCalculationService = tweetCalculationService ?? throw new ArgumentNullException(nameof(tweetCalculationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public ActionResult<TweetStatistics> GetCurrentTweetStatistics()
    {
        var tweetStatistics = _tweetCalculationService.GetTweetStreamStatistics();

        if (tweetStatistics is null) return NotFound(tweetStatistics);
        
        return Ok(tweetStatistics);
    }
}