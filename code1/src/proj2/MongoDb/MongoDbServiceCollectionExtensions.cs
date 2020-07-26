using System;
using HealthChecks.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using proj2.MongoDb;

namespace proj2
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration,
            IHealthChecksBuilder healthCheckBuilder)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection("MongoDb"))
                .AddSingleton<MongoDbHostedService2>()
                .AddSingleton(p => p.GetRequiredService<MongoDbHostedService2>().Client)
                .AddSingleton(p => p.GetRequiredService<MongoDbHostedService2>().Database)
                .AddSingleton<IHostedService>(p => p.GetRequiredService<MongoDbHostedService2>());


            healthCheckBuilder.Add(new HealthCheckRegistration("mongodb",
                p =>
                {
                    var svc = p.GetRequiredService<MongoDbHostedService2>();
                    return new MongoDbHealthCheck(svc.Client.Settings, svc.Options.DatabaseId);
                },
                default,
                new[] {"services"},
                TimeSpan.FromSeconds(1)));
        }
    }
}