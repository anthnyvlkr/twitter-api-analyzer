using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Tweetinvi.Models.V2;
using Xunit;

namespace TwitterAPI.Analyzer.API.Tests.Controllers.TweetControllerTests;

[ExcludeFromCodeCoverage]
public class GetRandomTweet : TweetControllerTests
{
    [Fact]
    public async Task RandomTweetFound_Should_Return_200()
    {
        // arrange
        var randomTweetResponse = TweetResponseFaker.Generate();

        _mockTwitterClientService.GetRandomTweet()!.Returns(Task.FromResult(randomTweetResponse));
        
        // act
        var result = await _sut.GetRandomTweet();
        
        // assert
        result.Result.As<OkObjectResult>().StatusCode.Should().Be((int) HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task RandomTweetFound_Should_Return_RandomTweet()
    {
        // arrange
        var randomTweetResponse = TweetResponseFaker.Generate();

        _mockTwitterClientService.GetRandomTweet()!.Returns(Task.FromResult(randomTweetResponse));
        
        // act
        var result = await _sut.GetRandomTweet();
        
        // assert
        result.Result.As<ObjectResult>().Value.As<TweetV2>().Should().Be(randomTweetResponse.Tweet);
    }
    
    [Fact]
    public async Task NullTweetResponse_Should_Return_204()
    {
        // arrange
        _mockTwitterClientService.GetRandomTweet().ReturnsNull();
        
        // act
        var result = await _sut.GetRandomTweet();
        
        // assert
        result.Result.As<StatusCodeResult>().StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
    }
}