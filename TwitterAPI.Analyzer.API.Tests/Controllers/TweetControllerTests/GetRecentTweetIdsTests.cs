using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace TwitterAPI.Analyzer.API.Tests.Controllers.TweetControllerTests;

[ExcludeFromCodeCoverage]
public class GetRecentTweetIdsTests : TweetControllerTests
{
    [Fact]
    public void FoundTweetIds_Should_Return_200()
    {
        // arrange
        var randomTweetIds = _faker.Random.WordsArray(10);
        _mockTwitterClientService.GetRecentTweetIds().Returns(randomTweetIds);
        
        // act
        var result = _sut.GetRecentTweetIds();
        
        // assert
        result.Result.As<OkObjectResult>().StatusCode.Should().Be((int) HttpStatusCode.OK);
    }
    
    [Fact]
    public void FoundTweetIds_Should_Return_FoundTweetIds()
    {
        // arrange
        var randomTweetIds = _faker.Random.WordsArray(10);
        _mockTwitterClientService.GetRecentTweetIds().Returns(randomTweetIds);
        
        // act
        var result = _sut.GetRecentTweetIds();
        
        // assert
        result.Result.As<OkObjectResult>().Value.Should().Be(randomTweetIds);
    }
    
    [Fact]
    public void EmptyTweetIds_Should_Return_204()
    {
        // arrange
        _mockTwitterClientService.GetRecentTweetIds().Returns(Array.Empty<string>());
        
        // act
        var result = _sut.GetRecentTweetIds();
        
        // assert
        result.Result.As<StatusCodeResult>().StatusCode.Should().Be((int) HttpStatusCode.NoContent);
    }
    
    [Fact]
    public void NullTweetIds_Should_Return_500()
    {
        // arrange
        _mockTwitterClientService.GetRecentTweetIds().ReturnsNull();
        
        // act
        var result = _sut.GetRecentTweetIds();
        
        // assert
        result.Result.As<StatusCodeResult>().StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
    }
}