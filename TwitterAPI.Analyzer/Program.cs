using Microsoft.Extensions.Hosting;
using Serilog;

namespace TwitterAPI.Analyzer;

public partial class Program
{
    public static async Task Main()
    {
        IHost? host = null;
        Task? hostTask = null;
        var cancellationTokenSource = new CancellationTokenSource();
        
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
        
        try
        {
            // read appsettings and build logger
            var configuration = BuildConfiguration();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Console.WriteLine("Press any key to connect to twitter sample stream...");
            Console.WriteLine("Press any key to quit the application...");
            Console.ReadLine();
            Console.WriteLine("Connecting to Twitter Sample StreamV2...");
            
            host = CreateHostBuilder(configuration).Build();
            hostTask = host.StartAsync(cancellationTokenSource.Token);

            Console.ReadKey();
            cancellationTokenSource.Cancel();
            Console.WriteLine("Stopping application...");

            await hostTask.WaitAsync(cancellationTokenSource.Token);
            
            Console.WriteLine("Complete...");
        }
        catch (Exception e)
        {
            Log.Logger.Fatal(e, "The application unexpectedly crashed");
            Environment.Exit(Environment.ExitCode);
        }   
        finally
        {
            if (hostTask is {Status: TaskStatus.Running} && !cancellationTokenSource.IsCancellationRequested) 
                cancellationTokenSource.Cancel();
            
            if (host != null) 
                await host.StopAsync(cancellationTokenSource.Token);

            Console.WriteLine("Exiting...");
            Log.CloseAndFlush();
        }
    }
}