using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace proj2
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiPathBase(Configuration);
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //  app.UseDefaultFiles().UseStaticFiles();

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapDefaultHealthChecks();
            });

            app.UseSwagger(MySwaggerDocs.GetSwaggerDocs());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddOptions();

            var healthCheckBuilder = services.ConfigureHealthChecks();
            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));

            services.ConfigureMongoDb(Configuration, healthCheckBuilder);
            services.ConfigureEventStore(Configuration, healthCheckBuilder);

            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddSwagger(MySwaggerDocs.GetSwaggerDocs(), (s, description) => true);
        }
    }
}