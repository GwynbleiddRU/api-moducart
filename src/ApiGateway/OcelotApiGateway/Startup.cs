using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
        private const string CorsPolicyName = "CorsPolicy";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // 🔹 CORS Policy - Restrict to allowed origins
            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicyName,
                    builder =>
                    {
                        builder
                            .WithOrigins(_configuration["Cors:AllowedOrigins"].Split(','))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                );
            });

            // 🔹 Authentication (JWT)
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    "Bearer",
                    options =>
                    {
                        options.Authority = _configuration["IdentityServer:Url"];
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _configuration["IdentityServer:Issuer"],
                            ValidAudience = _configuration["IdentityServer:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
                            ),
                        };
                    }
                );

            // 🔹 Swagger for Ocelot
            services.AddSwaggerForOcelot(_configuration);

            // 🔹 Ocelot with Consul, Polly, and Caching
            services
                .AddOcelot(_configuration)
                .AddConsul()
                .AddCacheManager(x => x.WithDictionaryHandle())
                .AddPolly();

            // 🔹 Health Checks
            services.AddHealthChecks();

            // 🔹 API Behavior Configuration
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // 🔹 Enable CORS
            app.UseCors(CorsPolicyName);

            app.UseRouting();

            // 🔹 Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // 🔹 Swagger UI for Ocelot
            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.ReConfigureUpstreamSwaggerJson = AlterUpstreamSwaggerJson;
            });

            // 🔹 Health checks endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // 🔹 Ocelot Middleware
            app.UseOcelot().Wait();
        }

        // 🔹 Custom Swagger JSON Modifier
        private string AlterUpstreamSwaggerJson(HttpContext context, string swaggerJson)
        {
            var swagger = JObject.Parse(swaggerJson);
            return swagger
                .ToString(Formatting.Indented)
                .Replace("localhost", _configuration["ApiGateway:ExternalUrl"]);
        }
    }
}
