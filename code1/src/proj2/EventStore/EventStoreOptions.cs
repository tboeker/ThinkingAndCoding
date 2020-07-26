using System;
using EventStore.ClientAPI;
using JC = Newtonsoft.Json.JsonConvert;
using JI = Newtonsoft.Json.JsonIgnoreAttribute;
using JI1 = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace proj2.EventStore
{
    public class EventStoreOptions
    {
        /// <summary>
        ///     Verbindung zum EventStore - IP Adresse
        /// </summary>
        public string AdminPassword { get; set; } = "changeit";

        /// <summary>
        ///     Verbindung zum EventStore - IP Adresse
        /// </summary>
        public string AdminUsername { get; set; } = "admin";

        public string ConnectionName { get; set; } = "proj2";
        //public EventStoreOptions()
        //{
        //    ClusterIpEndpoints = new string[0];
        //    ClusterHttpPorts = new int[0];
        //}

        /// <summary>
        ///     Customizen der Connection Settings
        /// </summary>
        [JI]
        [JI1]
        public Action<ConnectionSettingsBuilder> ConnectionSetup { get; set; }

        /// <summary>
        ///     Port für die Sichere Tcp Verbindung
        /// </summary>
        public int ExtSecureTcpPort { get; set; } = 1115;

        /// <summary>
        ///     Port f�r HTTP Verbidnung
        /// </summary>
        public int HttpPort { get; set; } = 2113;

        public double InitialPollingDelaySeconds { get; set; } = 0.2;

        /// <summary>
        ///     Verbindung zum EventStore - IP Adresse
        /// </summary>
        public string IpEndPoint { get; set; } = "127.0.0.1";

        public int MaximumPollingDelaySeconds { get; set; } = 30;

        //public bool UseCluster { get; set; }

        //public string[] ClusterIpEndpoints { get; set; }

        //public int[] ClusterHttpPorts { get; set; }

        public double OperationTimeoutSeconds { get; set; } = 5;

        /// <summary>
        ///     Verbindung zum EventStore - IP Adresse
        /// </summary>
        public string OpsPassword { get; set; } = "changeit";

        /// <summary>
        ///     Verbindung zum EventStore - IP Adresse
        /// </summary>
        public string OpsUsername { get; set; } = "ops";

        public double ProjectionOperationTimeoutSeconds { get; set; } = 30;

        public double QueryTimeoutSeconds { get; set; } = 30;

        public int ReadPageSize { get; set; } = 100;

        /// <summary>
        ///     Target Host für SSL
        /// </summary>
        public string SslTargetHost { get; set; } = "softwarepioniere_dev";

        /// <summary>
        ///     Server Validierung für SSL aktiv
        /// </summary>
        public bool SslValidateServer { get; set; }


        /// <summary>
        ///     Verbindung zum EventStore - Port
        /// </summary>
        public int TcpPort { get; set; } = 1113;

        /// <summary>
        ///     Gibt an, ob ssl verwendet werden sollen
        /// </summary>
        public bool UseSslCertificate { get; set; }

        public int WritePageSize { get; set; } = 50;

        public EventStoreOptions CreateSecured()
        {
            var json = JC.SerializeObject(this);
            var opt = JC.DeserializeObject<EventStoreOptions>(json);
            opt.AdminPassword = "XXX";
            opt.OpsPassword = "XXX";
            return opt;
        }
    }
}