using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace proj1
{
    public class DelayHostedService : IHostedService
    {
        private readonly ILogger<DelayHostedService> _logger;
        private readonly IConfiguration _configuration;

        public DelayHostedService(ILogger<DelayHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var delayConfigValue = _configuration["DelaySeconds"];

            var delay = TimeSpan.FromSeconds(10);

            if (!string.IsNullOrEmpty(delayConfigValue))
            {
                if (int.TryParse(delayConfigValue, out var i))
                {
                    _logger.LogInformation("Delay Seconds from config: {DelaySeconds}", delayConfigValue);
                    delay = TimeSpan.FromSeconds(i);    
                }
            }

            _logger.LogInformation("Starting with Delay: {delay}", delay);
            await Task.Delay(delay, cancellationToken);
            _logger.LogInformation("Delay Finished");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync");
            return Task.CompletedTask;
        }
    }
}