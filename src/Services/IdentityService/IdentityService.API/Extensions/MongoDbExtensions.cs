using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace IdentityService.API.Extensions
{
    public static class MongoDbExtensions
    {
        public class MongoDbSettings
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }

        public static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Get configuration section
            var configSection = configuration.GetSection("MongoDbSettings");

            // Manually bind and validate settings
            var settings = new MongoDbSettings
            {
                ConnectionString = configSection["ConnectionString"],
                DatabaseName = configSection["DatabaseName"],
            };

            // Validate settings
            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ApplicationException("MongoDB ConnectionString is not configured");
            if (string.IsNullOrWhiteSpace(settings.DatabaseName))
                throw new ApplicationException("MongoDB DatabaseName is not configured");

            // Register services
            services.AddSingleton(settings); // Direct instance
            services.AddSingleton<IMongoClient>(new MongoClient(settings.ConnectionString));

            // Register database instance with validation
            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var config = sp.GetRequiredService<MongoDbSettings>();
                return client.GetDatabase(config.DatabaseName);
            });

            return services;
        }
    }
}
