using Serilog;
using TwitterAPI.Analyzer.API.Middleware;
using TwitterAPI.Analyzer.API.Services;
using TwitterAPI.Analyzer.Common.BackgroundServices;
using TwitterAPI.Analyzer.Common.Configuration;
using TwitterAPI.Analyzer.Common.Factory;
using TwitterAPI.Analyzer.Common.Services;
using TwitterAPI.Analyzer.Storage.FakeDb;
using TwitterAPI.Analyzer.Storage.Repository;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{environment}.json", optional: false)
    .AddUserSecrets<Program>()
    .Build();

// build logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

// setup dependencies for controllers
builder.Services.AddControllers();

// swagger config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Background Services
builder.Services.AddHostedService<TwitterStreamWorker>()
    .AddLogging(hostBuilder => hostBuilder.AddSerilog(Log.Logger));

// Service dependencies
builder.Services.AddSingleton<ITweetCalculationService, TweetCalculationService>();
builder.Services.AddSingleton<ITwitterClientFactory, TwitterClientFactory>();
builder.Services.AddSingleton<ITwitterClientService, TwitterClientService>();
builder.Services.AddSingleton<ITwitterRepository, TwitterRepository>();
builder.Services.AddSingleton<IFakeDb, FakeDb>();

builder.Services.AddSingleton<ILogger>(provided => Log.Logger);

// options
builder.Services.Configure<TwitterClientConfiguration>(configuration.GetSection(TwitterClientConfiguration.Section));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();