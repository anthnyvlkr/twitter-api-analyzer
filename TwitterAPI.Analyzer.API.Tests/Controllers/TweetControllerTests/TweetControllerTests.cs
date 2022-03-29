using System.Diagnostics.CodeAnalysis;
using AutoBogus;
using Bogus;
using NSubstitute;
using Tweetinvi.Models.V2;
using TwitterAPI.Analyzer.API.Controllers;
using TwitterAPI.Analyzer.API.Services;

namespace TwitterAPI.Analyzer.API.Tests.Controllers.TweetControllerTests;

[ExcludeFromCodeCoverage]
public class TweetControllerTests
{
    protected readonly TweetController _sut;
    protected readonly ITwitterClientService _mockTwitterClientService;
    protected readonly Faker _faker;
    
    protected static Faker<TweetV2Response> TweetResponseFaker => new AutoFaker<TweetV2Response>()
        .RuleFor(c => c.Tweet, TweetFaker);
    
    
    protected static Faker<TweetV2> TweetFaker => new AutoFaker<TweetV2>();

    public TweetControllerTests()
    {
        _mockTwitterClientService = Substitute.For<ITwitterClientService>();
        _faker = new Faker();
        
        _sut = new TweetController(_mockTwitterClientService);
    }
}