using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace TwitterAPI.Analyzer.API.Tests.Controllers.TweetControllerTests;

[ExcludeFromCodeCoverage]
public class GetTweetByIdAsync : TweetControllerTests
{
    [Fact]
    public async Task TweetFound_Should_Return_200()
    {
        // arrange
        var randomTweetId = _faker.Random.String2(10);
        var randomTweet = TweetFaker
            .RuleFor(p => p.Id, randomTweetId)
            .Generate();
        
        var randomTweetResponse = TweetResponseFaker
            .RuleFor(p => p.Tweet, randomTweet)
            .Generate();

        _mockTwitterClientService.GetTweetByIdAsync(Arg.Any<string>())!
            .Returns(Task.FromResult(randomTweetResponse));
        
        // act
        var result = await _sut.GetTweetByIdAsync(randomTweetId);
        
        // assert
        result.Result.As<OkObjectResult>().StatusCode.Should().Be((int) HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task TweetFound_Should_Return_TweetFound()
    {
        // arrange
        var randomTweetId = _faker.Random.String2(10);
        var randomTweet = TweetFaker
            .RuleFor(p => p.Id, randomTweetId)
            .Generate();
        
        var randomTweetResponse = TweetResponseFaker
            .RuleFor(p => p.Tweet, randomTweet)
            .Generate();

        _mockTwitterClientService.GetTweetByIdAsync(Arg.Any<string>())!
            .Returns(Task.FromResult(randomTweetResponse));
        
        // act
        var result = await _sut.GetTweetByIdAsync(randomTweetId);
        
        // assert
        result.Result.As<OkObjectResult>().Value.Should().Be(randomTweet);
    }

    [Fact]
    public async Task NullTweetResponse_Should_Return_404()
    {
        // arrange
        var randomTweetId = _faker.Random.String2(10);
        _mockTwitterClientService.GetTweetByIdAsync(Arg.Any<string>()).ReturnsNull();
        
        // act
        var result = await _sut.GetTweetByIdAsync(randomTweetId);
        
        // assert
        result.Result.As<NotFoundResult>().StatusCode.Should().Be((int) HttpStatusCode.NotFound);
    }
}