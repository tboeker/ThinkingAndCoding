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

// ReSharper disable MemberCanBePrivate.Global

namespace proj2.MongoDb
{
    public class MongoDbHostedService : IHostedService
    {
        private readonly ILogger<MongoDbHostedService> _logger;

        public MongoDbHostedService(ILogger<MongoDbHostedService> logger, IOptions<MongoDbOptions> options)
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


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connecting to MongoDb with {@Options}",
                Options.CreateSecured());

            await CheckDatabaseExistsAsync(cancellationToken).ConfigureAwait(false);

            var client = CreateClient();

            Database = client.GetDatabase(Options.DatabaseId);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

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
    }
}