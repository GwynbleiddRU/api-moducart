using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public static class MongoDbExtensions
{
    public class MongoDbSettings
    {
        [Required]
        public string ConnectionString { get; set; } = string.Empty;

        [Required]
        public string DatabaseName { get; set; } = string.Empty;
    }

    public static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure MongoDB settings
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        // Register MongoDB client
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        // Register MongoDB database
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return client.GetDatabase(settings.DatabaseName);
        });

        return services;
    }
}
