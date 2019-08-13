using System.Configuration;

namespace Shuttle.Esb.NetMQ.Server.Configuration
{
    public class QueueElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)this["name"];

        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri => (string)this["uri"];
    }
}