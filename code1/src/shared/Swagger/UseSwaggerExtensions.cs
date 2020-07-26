using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class UseSwaggerExtensions
    {
        public static void UseSwagger(this IApplicationBuilder app, MySwaggerDoc[] docs)
        {
            var logger =
                app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Startup:UseSwagger");

            string GetRouteTemplate()
            {
                return "swagger/{documentName}.json";
            }

            string GetUrl(MySwaggerDoc doc)
            {
                return $"./{doc.Name}.json";
            }

            app.UseSwagger(o =>
                {
                    var routeTemplate = GetRouteTemplate();
                    logger.LogInformation("RouteTemplate: {Template}", routeTemplate);
                    o.RouteTemplate = routeTemplate;

                    // o.PreSerializeFilters.Add((document, request) => { document.Servers.Clear(); });
                })
                .UseSwaggerUI(o =>
                {
                    o.RoutePrefix = "swagger";
                    foreach (var doc in docs)
                    {
                        logger.LogInformation("Adding SwaggerEndpoint: {Name}", doc.Name);
                        o.SwaggerEndpoint(GetUrl(doc), doc.Name);
                    }

                    // var sm = new SubmitMethod[0];
                    // o.SupportedSubmitMethods(sm);
                    // o.EnableFilter();
                });
        }
    }
}