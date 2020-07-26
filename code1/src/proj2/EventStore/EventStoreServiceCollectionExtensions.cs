using System;
using HealthChecks.EventStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using proj2.EventStore;

namespace proj2
{
    public static class EventStoreServiceCollectionExtensions
    {
        public static void ConfigureEventStore(this IServiceCollection services, IConfiguration configuration,
            IHealthChecksBuilder healthCheckBuilder)
        {
            services
                .Configure<EventStoreOptions>(configuration.GetSection("EventStore"))
                .AddSingleton<EventStoreHostedService2>()
                .AddSingleton(p => p.GetRequiredService<EventStoreHostedService2>().Connection)
                .AddSingleton<IHostedService>(p => p.GetRequiredService<EventStoreHostedService2>());

            healthCheckBuilder
                .Add(new HealthCheckRegistration("eventstore",
                    p =>
                    {
                        var svc = p.GetRequiredService<EventStoreHostedService2>();
                        return new EventStoreHealthCheck(svc.ConnectionString,
                            svc.Options.AdminUsername,
                            svc.Options.AdminPassword);
                    },
                    default,
                    new[] {"services"},
                    TimeSpan.FromSeconds(1)));
        }
    }
}