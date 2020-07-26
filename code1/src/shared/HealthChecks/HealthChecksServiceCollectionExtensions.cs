using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class HealthChecksServiceCollectionExtensions
    {
        public static IHealthChecksBuilder ConfigureHealthChecks(this IServiceCollection services)
        {
            return services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
        }

        public static void MapDefaultHealthChecks(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckReponseWriter.WriteHealthCheckResponse
                });

            endpoints.MapHealthChecks("/health/self",
                new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self"),
                    ResponseWriter = HealthCheckReponseWriter.WriteHealthCheckResponse
                });

            endpoints.MapHealthChecks("/health/ready",
                new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("services"),
                    ResponseWriter = HealthCheckReponseWriter.WriteHealthCheckResponse
                });

            endpoints.MapHealthChecks("/healthz",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        }
    }
}