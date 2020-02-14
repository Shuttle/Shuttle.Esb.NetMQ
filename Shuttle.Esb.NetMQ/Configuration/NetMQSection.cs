using System;
using System.Configuration;
using Shuttle.Core.Configuration;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQSection : ConfigurationSection
    {
        [ConfigurationProperty("requestTimeout", IsRequired = false, DefaultValue = "00:00:01")]
        public TimeSpan RequestTimeout => TimeSpan.Parse((string) this["requestTimeout"]);

        public static INetMQConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<NetMQSection>("shuttle", "netmq");

            var result = new NetMQConfiguration();

            if (section != null)
            {
                result.RequestTimeout = section.RequestTimeout;
            }

            return result;
        }
    }
}