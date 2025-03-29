using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdentityService.API 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // var logPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Logs", ".log");
            // Log.Logger = new LoggerConfiguration().ReadFrom
            //     .Configuration(configuration)
            //     .WriteTo.File(
            //         logPath,
            //         rollingInterval: RollingInterval.Day,
            //         outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            //     )
            //     .CreateLogger();

            // try
            // {
            //     Log.Information("Starting up the host");
                CreateHostBuilder(args).Build().Run();
            // }
            // catch (Exception ex)
            // {
            //     Log.Fatal(ex, "Host terminated unexpectedly");
            // }
            // finally
            // {
            //     Log.CloseAndFlush();
            // }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}