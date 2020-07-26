using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using proj1.Lifetime;

namespace proj1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DateTime upTime = DateTime.UtcNow;
            
            app.UseApiPathBase(Configuration);
            app.UseForwardedHeaders();

            app.Use(async (context, next) =>
            {
                context.Items.Add("UpSince", upTime);
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            //app.UseDefaultFiles().UseStaticFiles();
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


            var delayConfigValue = Configuration["DelaySeconds"];

            var delay = TimeSpan.FromSeconds(10);

            if (!string.IsNullOrEmpty(delayConfigValue))
            {
                if (int.TryParse(delayConfigValue, out var i))
                {
                    delay = TimeSpan.FromSeconds(i);
                }
            }

            var readyAtUtc = DateTime.UtcNow.Add(delay);

            services.ConfigureHealthChecks()
                .AddCheck("delay",
                    () =>
                    {
                        if (readyAtUtc <= DateTime.UtcNow)
                            return HealthCheckResult.Healthy($"is ready since: {readyAtUtc}");

                        return HealthCheckResult.Unhealthy($"will be ready at: {readyAtUtc}");
                    },
                    new[] {"services"})
                .AddCheck<DelayHealthCheck>("delay_svc", tags: new[] {"services"})
                ;

            services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin()));
            services.AddControllersWithViews();

            services
                .AddSingleton<MyApplicationLifetimeHostedService>()
                .AddSingleton<IMyApplicationLifetime>(p => p.GetRequiredService<MyApplicationLifetimeHostedService>())
                .AddSingleton<IHostedService>(p => p.GetRequiredService<MyApplicationLifetimeHostedService>())
                ;

            //services.AddHostedService<DelayHostedService>();
            services
                .AddSingleton<DelayHostedBackgroundService>()
                .AddSingleton<IHostedService>(p => p.GetRequiredService<DelayHostedBackgroundService>());

            services.AddSwagger(MySwaggerDocs.GetSwaggerDocs());
        }
    }

    public class DelayHealthCheck : IHealthCheck
    {
        private readonly DelayHostedBackgroundService _svc;

        public DelayHealthCheck(DelayHostedBackgroundService svc)
        {
            _svc = svc;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (_svc.IsStarted)
                return Task.FromResult(HealthCheckResult.Healthy());

            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}