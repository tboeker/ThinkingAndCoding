using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace hello
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiPathBase(Configuration);
            app.UseForwardedHeaders();
            
            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            app.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                string body = string.Empty;
                if (context.Request.Body != null)
                {
                    using (var stream = new StreamReader(context.Request.Body))
                    {
                        body = await stream.ReadToEndAsync();
                        // body = "param=somevalue&param2=someothervalue"
                    }
                }

                var res = new
                {
                    ApiPathBase = Configuration.GetApiPathBase(),
                    context.TraceIdentifier,
                    context.Request.Method,
                    Headers = context.Request.Headers.Keys.Select(k => new
                    {
                        Key = k,
                        Values = context.Request.Headers[k]
                    }),
                    Query = context.Request.Query?.Select(q => new
                    {
                        q.Key,
                        q.Value
                    }),
                    Body = body
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };


                await context.Response.WriteAsync(JsonSerializer.Serialize(res, options));
            });
        }
    }
}