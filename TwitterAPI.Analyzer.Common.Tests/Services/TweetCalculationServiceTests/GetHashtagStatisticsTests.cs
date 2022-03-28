using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TwitterAPI.Analyzer.Common.Models;
using TwitterAPI.Analyzer.Storage.Exceptions;
using Xunit;

namespace TwitterAPI.Analyzer.Common.Tests.Services.TweetCalculationServiceTests;

[ExcludeFromCodeCoverage]
public class GetHashtagStatisticsTests : TweetCalculationServiceTests
{
    [Fact]
    public void ThreeHashTags_Should_Return_OrderedByCount()
    {
        // arrange 
        var hashtag1 = _faker.Random.String2(6);
        var hashtag2 = _faker.Random.String2(6);
        var hashtag3 = _faker.Random.String2(6);
        var count1 = _faker.Random.Long(0, 10);
        var count2 = _faker.Random.Long(100, 150);
        var count3 = _faker.Random.Long(11, 50);

        var randomDictionary = new Dictionary<string, long>
        {
            {hashtag1, count1},
            {hashtag2, count2 },
            {hashtag3, count3 },
        };

        var expected = new List<HashtagStatistics>
        {
            new()
            {
                HashTag = hashtag2,
                Count = count2
            },
            new()
            {
                HashTag = hashtag3,
                Count = count3
            },
            new ()
            {
                HashTag = hashtag1,
                Count = count1
            },
        };
        
        _mockTwitterRepo.GetHashtags().Returns(randomDictionary);
        
        // act
        var result = _sut.GetHashtagStatistics();
        
        // assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void NoHashTags_Should_Log()
    {
        // arrange 
        _mockTwitterRepo.GetHashtags().Returns(new Dictionary<string, long>());
        
        // act
        _ = _sut.GetHashtagStatistics();
        
        // assert
        _mockLogger.Received(2)
            .Verbose(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Fact]
    public void NoHashTags_Should_Return_Empty()
    {
        // arrange 
        _mockTwitterRepo.GetHashtags().Returns(new Dictionary<string, long>());
        
        // act
        var result = _sut.GetHashtagStatistics();
        
        // assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void NullHashTags_Should_Return_Null()
    {
        // arrange 
        _mockTwitterRepo.GetHashtags().ReturnsNull();
        
        // act
        var result = _sut.GetHashtagStatistics();
        
        // assert
        result.Should().BeNull();
    }
    
    [Fact]
    public void ExceptionThrown_Should_Return_Null()
    {
        // arrange 
        _mockTwitterRepo.GetHashtags().Throws(new TwitterRepositoryException());
        
        // act
        var result = _sut.GetHashtagStatistics();
        
        // assert
        result.Should().BeNull();
    }
}