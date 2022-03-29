using System.Diagnostics.CodeAnalysis;
using Bogus;
using NSubstitute;
using Serilog;
using TwitterAPI.Analyzer.Common.Services;
using TwitterAPI.Analyzer.Storage.Repository;

namespace TwitterAPI.Analyzer.Common.Tests.Services.TweetCalculationServiceTests;

[ExcludeFromCodeCoverage]
public class TweetCalculationServiceTests
{
    protected readonly TweetCalculationService _sut;
    protected readonly ITwitterRepository _mockTwitterRepo;
    protected readonly IStopwatchService _mockStopwatchService;
    protected readonly ILogger _mockLogger;
    
    protected Faker _faker;
    
    public TweetCalculationServiceTests()
    {
        _mockTwitterRepo = Substitute.For<ITwitterRepository>();
        _mockLogger = Substitute.For<ILogger>();
        _mockStopwatchService = Substitute.For<IStopwatchService>();
        _faker = new Faker();
        
        _sut = new TweetCalculationService(_mockTwitterRepo, _mockStopwatchService, _mockLogger);
    }
}