using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
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

            // Configure SwaggerForOcelot before Ocelot
            services.AddSwaggerForOcelot(Configuration);

            services
                .AddOcelot(Configuration)
                .AddConsul()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                .AddPolly();
            // .AddTransientDefinedAggregator<ServicesAggregator>();

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

            // Configure Swagger for Ocelot
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.ReConfigureUpstreamSwaggerJson = AlterUpstreamSwaggerJson;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseOcelot().Wait();
        }

        private string AlterUpstreamSwaggerJson(HttpContext context, string swaggerJson)
        {
            var swagger = JObject.Parse(swaggerJson);
            // Replace localhost with yourecommerce.com
            return swagger.ToString(Formatting.Indented).Replace("localhost", "yourecommerce.com");
        }
    }

    // // Sample aggregator class to combine results from different services if needed
    // public class ServicesAggregator : Ocelot.Middleware.Multiplexer.IDefinedAggregator
    // {
    //     public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    //     {
    //         // Implement aggregation logic here if needed
    //         return await Task.FromResult<DownstreamResponse>(null);
    //     }
    // }
}
