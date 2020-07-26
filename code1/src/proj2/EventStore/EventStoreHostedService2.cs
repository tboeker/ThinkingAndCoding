using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace proj2.EventStore
{
    public class EventStoreHostedService2 : BackgroundService
    {
        private readonly ILogger<EventStoreHostedService2> _logger;
        
        public EventStoreHostedService2(ILogger<EventStoreHostedService2> logger, IOptions<EventStoreOptions> options)
        {
            _logger = logger;
            Options = options.Value;

            OpsCredentials = new UserCredentials(Options.OpsUsername, Options.OpsPassword);
            AdminCredentials = new UserCredentials(Options.AdminUsername, Options.AdminPassword);

            var connectionSettingsBuilder = ConnectionSettings.Create()
                    .KeepReconnecting()
                    .KeepRetrying()
                    .UseCustomLogger(new EventStoreLogger(logger))
                ;

            Options.ConnectionSetup?.Invoke(connectionSettingsBuilder);

            //var url = $"tcp://{Options.IpEndPoint}:{Options.TcpPort}";
            //var uri = new Uri(url);
            //Connection = EventStoreConnection.Create(connectionSettingsBuilder, uri, Options.ConnectionName);

            ConnectionString =
                $"ConnectTo=tcp://{Options.AdminUsername}:{Options.AdminPassword}@{Options.IpEndPoint}:{Options.TcpPort}";
            
            Connection = EventStoreConnection.Create(
                ConnectionString,
                connectionSettingsBuilder,
                Options.ConnectionName);

            Connection.Disconnected += (sender, args) =>
            {
                _logger.LogInformation("EventStore Disconnected: {ConnectionName}", args.Connection.ConnectionName);
            };

            Connection.Connected += (sender, args) =>
            {
                _logger.LogInformation("EventStore Connected: {ConnectionName}", args.Connection.ConnectionName);
            };

            Connection.Reconnecting += (sender, args) =>
            {
                _logger.LogInformation("EventStore Reconnecting: {ConnectionName}", args.Connection.ConnectionName);
            };

            Connection.Closed += (sender, args) =>
            {
                _logger.LogInformation("EventStore Closed: {ConnectionName}", args.Connection.ConnectionName);
            };
        }
        
        public UserCredentials AdminCredentials { get; }

        public IEventStoreConnection Connection { get; }
        
        public string ConnectionString { get; }

        public UserCredentials OpsCredentials { get; }

        public EventStoreOptions Options { get; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
            {
                _logger.LogInformation("Closing EventStore Connection {ConnectionName}", Connection.ConnectionName);
                Connection.Close();
            });
            
            _logger.LogInformation("Connecting to EventStore {ConnectionName} with {@Options}",
                Options.ConnectionName,
                Options.CreateSecured());
            return Connection.ConnectAsync();


            
        }
    }
}