using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Nats.Client.Api.HealthCheck
{
    /// <inheritdoc />
    internal class MemoryHealthCheck : IHealthCheck
    {
        /// <inheritdoc />
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            const long threshold = 1 * 1024 * 1024 * 1024;

            // Include GC information in the reported diagnostics.
            var allocated = GC.GetTotalMemory(false);
            var data = new Dictionary<string, object>
            {
                { "AllocatedBytes", allocated },
                { "AllocatedKBytes", allocated / 1024 },
                { "AllocatedMBytes", allocated / 1024 / 1024 },
                { "AllocatedGBytes", allocated / 1024 / 1024 / 1024 },
                { "Gen0Collections", GC.CollectionCount(0) },
                { "Gen1Collections", GC.CollectionCount(1) },
                { "Gen2Collections", GC.CollectionCount(2) }
            };

            var status = allocated < threshold ? HealthStatus.Healthy : HealthStatus.Degraded;

            return Task.FromResult(new HealthCheckResult(
                status,
                $"Reports degraded status if allocated bytes >= {threshold} bytes.",
                null,
                data));
        }
    }
}