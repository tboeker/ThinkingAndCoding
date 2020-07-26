using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace proj1.Lifetime
{
    internal class MyApplicationLifetimeHostedService : IHostedService, IMyApplicationLifetime
    {
        private readonly ILogger<MyApplicationLifetimeHostedService> _logger;

        public MyApplicationLifetimeHostedService(ILogger<MyApplicationLifetimeHostedService> logger,
            IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            lifetime.ApplicationStarted.Register(() =>
            {
                _logger.LogInformation("Application Started");
                IsStarting = false;
                IsStarted = true;
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                _logger.LogInformation("Application Stopping");
                IsStarting = false;
                IsStarted = false;
                IsStopping = true;
            });

            lifetime.ApplicationStopped.Register(() =>
            {
                _logger.LogInformation("Application Stopped");
                IsStopping = false;
                IsStopped = true;
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync");
            IsStarting = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync");
            return Task.CompletedTask;
        }

        public IHostApplicationLifetime HostApplicationLifetime => throw new NotImplementedException();

        public bool IsStarted { get; private set; }

        public bool IsStarting { get; private set; }

        public bool IsStopped { get; private set; }

        public bool IsStopping { get; private set; }
    }
}