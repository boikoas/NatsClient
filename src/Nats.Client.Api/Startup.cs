using Nats.Client.Api.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Nats.Client.Infrastructure.Middleware;
using Nats.Client.Infrastructure.Messaging.Nats;
using Nats.Client.Infrastructure.Serializers.Binary;
using Nats.Client.Infrastructure.Dispatchers;
using System;
using Nats.Client.Infrastructure.Database;
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