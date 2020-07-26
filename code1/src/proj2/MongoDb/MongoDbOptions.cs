using System;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json;

namespace proj2.MongoDb
{
    public class MongoDbOptions
    {
        [JsonIgnore]
        public Action<ClusterBuilder> ClusterConfigurator { get; set; }

        public string DatabaseId { get; set; } = "db1";

        public int? FindBatchSize { get; set; }

        public int? FindLimit { get; set; }

        public string Password { get; set; }

        public int Port { get; set; } = 27017;

        public string Server { get; set; } = "localhost";

        public string UserName { get; set; }

        public bool UseTelemetry { get; set; }

        //public override string ToString()
        //{
        //    return $"Server: {Server} // Port: {Port} // DatabaseId: {DatabaseId} // UserName: {UserName} ";
        //}

        public MongoDbOptions CreateSecured()
        {
            return new MongoDbOptions
            {
                DatabaseId = DatabaseId,
                Port = Port,
                Server = Server,
                UserName = UserName,
                Password = "XXX"
            };
        }
    }
}