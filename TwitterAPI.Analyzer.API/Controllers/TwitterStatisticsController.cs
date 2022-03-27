using System.Net;
using Microsoft.AspNetCore.Mvc;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Common.Services;

namespace TwitterAPI.Analyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TwitterStatisticsController : ControllerBase
{
    private readonly ITweetCalculationService _tweetCalculationService;

    public TwitterStatisticsController(
        ITweetCalculationService tweetCalculationService)
    {
        _tweetCalculationService = tweetCalculationService ?? throw new ArgumentNullException(nameof(tweetCalculationService));
    }

    [HttpGet]
    public ActionResult<TweetStatistics> GetCurrentTweetStatistics()
    {
        var tweetStatistics = _tweetCalculationService.GetTweetStreamStatistics();

        if (tweetStatistics is null) return NotFound(tweetStatistics);
        
        return Ok(tweetStatistics);
    }
    
    [HttpGet("hashtags")]
    public ActionResult<HashtagStatistics> GetHashtagStatistics()
    {
        var hashtagStatistics = _tweetCalculationService.GetHashtagStatistics();

        if (hashtagStatistics == null) return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        
        if (!hashtagStatistics.Any()) return NoContent();

        return Ok(hashtagStatistics);
    }
}