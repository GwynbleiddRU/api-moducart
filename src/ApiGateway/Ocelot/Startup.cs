using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.Configuration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

namespace ModuCart.ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "E-Commerce API Gateway",
                        Version = "v1",
                        Description = "API Gateway for E-Commerce Microservices",
                        Contact = new OpenApiContact
                        {
                            Name = "Your Name",
                            Email = "your.email@example.com",
                        },
                    }
                );
            });

            services.AddSwaggerForOcelot(Configuration);

            services
                .AddOcelot(Configuration)
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                .AddPolly()
                .AddTransientDefinedAggregator<ServicesAggregator>();

            // Add health checks
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            // Add Swagger UI
            app.UseSwagger();

            // Configure Swagger for Ocelot
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.ReConfigureUpstreamSwaggerJson = AlterUpstream;
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseOcelot().Wait();
        }

        private string AlterUpstream(
            HttpContext context,
            SwaggerForOcelotUIOptions options,
            string swaggerJson
        )
        {
            return swaggerJson.Replace("localhost", "yourecommerce.com");
        }
    }

    // Sample aggregator class to combine results from different services if needed
    public class ServicesAggregator
    {
        // Implement aggregation logic here if needed
    }
}
