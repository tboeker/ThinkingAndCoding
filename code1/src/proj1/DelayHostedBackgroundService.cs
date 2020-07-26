using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace proj1
{
    public class DelayHostedBackgroundService : BackgroundService
    {
        private readonly ILogger<DelayHostedBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        public bool IsStarted { get; private set; }

        public DelayHostedBackgroundService(ILogger<DelayHostedBackgroundService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
            await Task.Delay(delay, stoppingToken);
            IsStarted = true;
            _logger.LogInformation("Delay Finished");
            
            
        }
    }
}