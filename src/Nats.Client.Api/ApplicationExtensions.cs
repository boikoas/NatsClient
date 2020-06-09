using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nats.Client.Infrastructure.Services.ActionWriterService;
using System.Threading.Tasks;

namespace Nats.Client.Api
{
    public static class ApplicationExtensions
    {
        public static async Task<IApplicationBuilder> UseActionsSubscriptionAsync(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var actionReader = serviceScope.ServiceProvider.GetRequiredService<IActionWriterService>();
                await actionReader.BeginConsumeAsync();

                return app;
            }
        }
    }
}