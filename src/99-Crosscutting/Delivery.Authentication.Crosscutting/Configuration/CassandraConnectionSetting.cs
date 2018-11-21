namespace Delivery.Authentication.Crosscutting.Configuration
{
    public class CassandraConnectionSetting
    {
        public string[] Hosts { get; set; }

        public string Keyspace { get; set; }

        public string Password { get; set; }

        public string User { get; set; }

        public int Port { get; set; }
    }
}
