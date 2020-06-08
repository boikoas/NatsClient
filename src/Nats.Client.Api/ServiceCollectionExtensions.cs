using Nats.Client.Api.HealthCheck;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Nats.Client.Infrastructure.Database;
using Nats.Client.Infrastructure.Messaging.Nats;
using Nats.Client.Infrastructure.Database.Repositories;
using Nats.Client.Infrastructure.Serializers.Binary;
using Nats.Client.Infrastructure.Dispatchers;
using Nats.Client.Infrastructure.Services;
using Nats.Client.Infrastructure.Services.ActionWriterService;

namespace Nats.Client.Api
{
    /// <summary>
    /// ServiceCollectionExtensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавление сервисов приложения.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация <see cref="IConfiguration"/>.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) => services
            .AddOptions()
            .AddDbContext<AppDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddMvcActionFilters()
            .AddAllHealthChecks()
            .AddRepositories()
            .AddApplicationServices();

        /// <summary>
        /// Добавление MVC фильтров действий.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection AddMvcActionFilters(this IServiceCollection services) => services
            .Scan(scan =>
            {
                scan
                    .FromAssemblies(typeof(Startup).Assembly)
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IAsyncActionFilter)))
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(IAsyncResultFilter)))
                    .AsSelf()
                    .WithScopedLifetime();
            });

        /// <summary>
        /// Добавление хелсчеков.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection AddAllHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddCustomHealthChecks();

            return services;
        }

        /// <summary>
        /// Добавление репозиториев.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IMessageForSaveRepository, MessageForSaveRepository>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) => services
            .AddTransient<INatsConfiguration, NatsConfiguration>()
            .AddTransient<INatsManager, NatsManager>()
            .AddTransient<IBinarySerializer, BinaryFormatterSerializer>()
            .AddTransient<ICommandDispatcher, CommandDispatcher>()
            .AddTransient<IMessageProcessingService, MessageProcessingService>()
            .AddTransient<IActionWriterService, ActionWriterService>();

    }
}