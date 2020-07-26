using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace proj2.MongoDb
{
    public class MongoDbHostedService2 : BackgroundService
    {
        private readonly ILogger<MongoDbHostedService2> _logger;

        public MongoDbHostedService2(ILogger<MongoDbHostedService2> logger, IOptions<MongoDbOptions> options)
        {
            _logger = logger;
            var serializer = new DateTimeSerializer(DateTimeKind.Utc, BsonType.Document);
            BsonSerializer.RegisterSerializer(typeof(DateTime), serializer);

            Options = options.Value;
            Client = CreateClient();
        }

        public IMongoClient Client { get; set; }

        public IMongoDatabase Database { get; private set; }

        public MongoDbOptions Options { get; }

        public async Task<bool> CheckDatabaseExistsAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace(nameof(CheckDatabaseExistsAsync));

            var client = CreateClient();

            var databases = await client.ListDatabasesAsync(cancellationToken).ConfigureAwait(false);

            while (await databases.MoveNextAsync(cancellationToken).ConfigureAwait(false))
            {
                var items = databases.Current.ToArray();
                var c = items.Count(x => x["name"] == Options.DatabaseId) > 0;

                if (c)
                {
                    return true;
                }
            }

            return false;
        }

        public IMongoClient CreateClient()
        {
            var url = new MongoServerAddress(Options.Server, Options.Port);
            var settings = new MongoClientSettings
            {
                Server = url
            };

            if (!string.IsNullOrEmpty(Options.UserName) && !string.IsNullOrEmpty(Options.Password))
            {
                settings.Credential =
                    MongoCredential.CreateCredential(Options.DatabaseId, Options.UserName, Options.Password);
            }


            if (Options.ClusterConfigurator != null)
            {
                settings.ClusterConfigurator = Options.ClusterConfigurator;
            }

            var client = new MongoClient(settings);
            return client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Connecting to MongoDb with {@Options}",
                Options.CreateSecured());


            var client = CreateClient();

            Database = client.GetDatabase(Options.DatabaseId);

            var exi = await CheckDatabaseExistsAsync(stoppingToken).ConfigureAwait(false);
            _logger.LogInformation("Database Exists: {Exists}", exi);
            if (!exi)
            {
                await CreateDatabaseAsync(stoppingToken).ConfigureAwait(false);
            }
        }

        private async Task CreateDatabaseAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Creating Database and Init Collection");
            try
            {
                await Database.CreateCollectionAsync("init", cancellationToken: stoppingToken);
                var col = Database.GetCollection<InitData>("init");
                await col.InsertOneAsync(new InitData()
                    {
                        CreatedOnUtc = DateTime.UtcNow
                    },
                    cancellationToken: stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }


        public class InitData
        {
            public DateTime CreatedOnUtc { get; set; }
        }
    }
}