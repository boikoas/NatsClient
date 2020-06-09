using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nats.Client.Api.HealthCheck;
using Nats.Client.Infrastructure.Middleware;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Nats.Client.Api
{
    /// <summary>
    /// Startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);
        }

        public async void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomHealthChecks();
            app.UseGlobalExceptionHandler();
            await app.UseActionsSubscriptionAsync();
        }

        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", false, true)
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
             //.AddSpringCloud()
             .AddEnvironmentVariables()
             .Build();
    }
}