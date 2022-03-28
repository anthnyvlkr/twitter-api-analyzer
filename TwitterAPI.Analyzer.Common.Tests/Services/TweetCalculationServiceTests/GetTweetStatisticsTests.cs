using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TwitterAPI.Analyzer.Common.Exceptions;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Storage.Exceptions;
using Xunit;

namespace TwitterAPI.Analyzer.Common.Tests.Services.TweetCalculationServiceTests;

[ExcludeFromCodeCoverage]
public class GetTweetStatisticsTests : TweetCalculationServiceTests
{
    [Fact]
    public void ExistingCount_Should_Return_Expected_Count_PerSecond_PerMinute_Runtime()
    {
        // arrange 
        var randomCount = _faker.Random.Long(10, 10000);
        var timeSpan = TimeSpan.FromMinutes(10);
        
        _mockTwitterRepo.GetTweetCount().Returns(randomCount);
        _mockStopwatchService.Elapsed.Returns(timeSpan);
        
        var expected = new TweetStatistics
        {
            TotalCount = randomCount,
            PerSecond = randomCount / timeSpan.TotalSeconds,
            PerMinute = randomCount / timeSpan.TotalMinutes,
            RunTime = timeSpan
        };
        
        // act
        var result = _sut.GetTweetStreamStatistics();
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void TwitterRepo_ExceptionThrown_Should_Throw_Exception()
    {
        // arrange 
        _mockTwitterRepo.GetTweetCount().Throws<TwitterRepositoryException>();
        
        // act
        Func<TweetStatistics> func = () => _sut.GetTweetStreamStatistics();
        
        // assert
        func.Should().ThrowExactly<TweetCalculationServiceException>();
    }
    
    [Fact]
    public void NoCount_0TimeSpan_Should_Log()
    {
        // arrange 
        _mockTwitterRepo.GetTweetCount().Returns(0);
        _mockStopwatchService.Elapsed.Returns(TimeSpan.Zero);
        
        // act
        _ = _sut.GetTweetStreamStatistics();
        
        // assert
        _mockLogger.Received(2)
            .Verbose(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
}