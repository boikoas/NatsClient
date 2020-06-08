using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Nats.Client.Api.HealthCheck
{
    /// <summary>
    /// Extensions for custom health checks.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class HealthCheckExtensions
    {
        /// <summary>
        /// Use custom health checks.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
        {
            app
                .UseHealthChecks("/health")
                .UseHealthChecks(
                    "/health/detail",
                    new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        AllowCachingResponses = false,
                        ResponseWriter = WriteResponseAsync
                    })
                .UseHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("ready"),
                    AllowCachingResponses = false
                })
                .UseHealthChecks("/health/live", new HealthCheckOptions
                {
                    // Exclude all checks and return a 200-Ok
                    Predicate = _ => false,
                    AllowCachingResponses = false
                });

            return app;
        }

        /// <summary>
        /// Add custom health checks.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>IHealthChecksBuilder instance.</returns>
        public static IHealthChecksBuilder AddCustomHealthChecks(this IServiceCollection services)
        {
            return services
                .AddHealthChecks()
                .AddCheck<MemoryHealthCheck>(
                    "memory-check",
                    HealthStatus.Degraded,
                    new[] { "memory" });
        }

        private static Task WriteResponseAsync(
            HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}