using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.API.Services;
using ILogger = Serilog.ILogger;

namespace TwitterAPI.Analyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TweetController : ControllerBase
{
    private readonly ITwitterClientService _twitterClientService;
    private readonly ILogger _logger;

    public TweetController(
        ITwitterClientService twitterClientService,
        ILogger logger)
    {
        _twitterClientService = twitterClientService ?? throw new ArgumentNullException(nameof(twitterClientService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("random")]
    public async Task<ActionResult<TweetV2>> GetCurrentTweetStatistics()
    {
        var response = await _twitterClientService.GetRandomTweet();

        if (response is null) return NotFound();
        
        return Ok(response.Tweet);
    }
}