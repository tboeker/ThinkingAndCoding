using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore;

namespace SwaggerUI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddOptions();

            // services.ConfigureHealthChecks();
        }

        private MySwaggerDoc[] GetSwaggerDocs()
        {
            var section = Configuration.GetSection("SwaggerDocs");
            if (section == null)
                return new MySwaggerDoc[0];

            var v = section.Get<MySwaggerDoc[]>();
            return v;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

            app.UseApiPathBase(Configuration);
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseDefaultFiles().UseStaticFiles();
            
            app.UseSwaggerUI(o =>
            {
                o.RoutePrefix = string.Empty;
                foreach (var doc in GetSwaggerDocs().OrderBy(x => x.Name))
                {
                    logger.LogInformation("Adding SwaggerEndpoint {Name} on {Url}", doc.Name, doc.Url);
                    o.SwaggerEndpoint(doc.Url, doc.Name);
                }

                // o.EnableFilter();
            });
        }
    }
}