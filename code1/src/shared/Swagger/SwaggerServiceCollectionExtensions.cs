using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Filters;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services, MySwaggerDoc[] docs,
            Func<string, ApiDescription, bool> predicate = null)
        {
            services.AddSwaggerGen(o =>
                {
                    foreach (var xDoc in docs)
                    {
                        o.SwaggerDoc(xDoc.Name,
                            new OpenApiInfo
                            {
                                Title = xDoc.Title,
                                Version = "v1"
                            });
                    }

                    o.EnableAnnotations();
                    o.OperationFilter<SummaryFromOperationFilter>();
                    o.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                    if (predicate == null)
                    {
                        o.DocInclusionPredicate((s, description) =>
                        {
#if DEBUG
                            Console.WriteLine($"DocInclusionPredicate: {s} | {description.GroupName}");
#endif
                            return string.Equals(s, description.GroupName, StringComparison.InvariantCultureIgnoreCase);
                        });
                    }
                    else
                    {
                        o.SwaggerGeneratorOptions.DocInclusionPredicate = predicate;
                    }
                })
                .AddSwaggerGenNewtonsoftSupport();
        }
    }
}