using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ModuCart.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (hostingContext, config) =>
                    {
                        config
                            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile(
                                $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                                optional: true,
                                reloadOnChange: true
                            )
                            .AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(
                        (hostingContext, logging) =>
                        {
                            logging.AddConfiguration(
                                hostingContext.Configuration.GetSection("Logging")
                            );
                            logging.AddConsole();
                            logging.AddDebug();
                        }
                    );
                });
    }
}
