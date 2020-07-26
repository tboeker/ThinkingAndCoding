using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseApiPathBase(this IApplicationBuilder app, IConfiguration configuration)
        {
            var pathBase = configuration.GetApiPathBase();

            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
                logger.LogInformation("Adding Path Base: {Path}", pathBase);
                app.UsePathBase($"/{pathBase.TrimStart('/')}");
            }
        }
    }
}