using System;
using System.Configuration;
using Shuttle.Core.Configuration;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQSection : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int)this["port"];

        [ConfigurationProperty("serializerType", IsRequired = false, DefaultValue = "")]
        public string SerializerType => (string)this["serializerType"];

        [ConfigurationProperty("queues", IsRequired = true)]
        public QueueElementCollection Queues => (QueueElementCollection)this["queues"];

        public static INetMQConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<NetMQSection>("shuttle", "netmq");

            if (section == null)
            {
                throw new ConfigurationErrorsException(Resources.ConfigurationSectionMissing);
            }
            
            var result = new NetMQConfiguration
            {
                Port = section.Port,
                SerializerType = section.SerializerType
            };

            if (section.Queues.Count < 1)
            {
                throw new ConfigurationErrorsException(Resources.QueuesEmpty);
            }

            foreach (QueueElement queueElement in section.Queues)
            {
                result.AddQueue(new QueueConfiguration(queueElement.Name, queueElement.Uri));
            }

            return result;
        }
    }
}