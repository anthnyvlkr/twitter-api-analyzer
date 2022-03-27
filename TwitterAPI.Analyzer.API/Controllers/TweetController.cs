using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.API.Services;

namespace TwitterAPI.Analyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TweetController : ControllerBase
{
    private readonly ITwitterClientService _twitterClientService;

    public TweetController(
        ITwitterClientService twitterClientService)
    {
        _twitterClientService = twitterClientService ?? throw new ArgumentNullException(nameof(twitterClientService));
    }
    
    [HttpGet("tweetIds")]
    public ActionResult<TweetV2> GetRecentTweetIds()
    {
        var recentTweetIds = _twitterClientService.GetRecentTweetIds();

        if (!recentTweetIds.Any()) return NoContent();
        
        return Ok(recentTweetIds);
    }

    [HttpGet]
    public async Task<ActionResult<TweetV2>> GetTweetByIdAsync(string tweetId)
    {
        var response = await _twitterClientService.GetTweetByIdAsync(tweetId);

        if (response is null) return NotFound();
        
        return Ok(response.Tweet);
    }
    
    [HttpGet("random")]
    public async Task<ActionResult<TweetV2>> GetCurrentTweetStatisticsAsync()
    {
        var response = await _twitterClientService.GetRandomTweet();

        if (response is null) return NotFound();
        
        return Ok(response.Tweet);
    }
    
}