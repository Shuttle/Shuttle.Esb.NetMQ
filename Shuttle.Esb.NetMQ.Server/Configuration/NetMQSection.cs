using System;
using System.Configuration;
using Shuttle.Core.Configuration;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQSection : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int) this["port"];

        [ConfigurationProperty("serializerType", IsRequired = false, DefaultValue = "")]
        public string SerializerType => (string) this["serializerType"];

        [ConfigurationProperty("requestTimeout", IsRequired = false, DefaultValue = "00:00:01")]
        public TimeSpan RequestTimeout => TimeSpan.Parse((string) this["requestTimeout"]);

        [ConfigurationProperty("queues", IsRequired = true)]
        public QueueElementCollection Queues => (QueueElementCollection) this["queues"];

        [ConfigurationProperty("queueFactories", IsRequired = false, DefaultValue = null)]
        public QueueFactoriesElement QueueFactories => (QueueFactoriesElement) this["queueFactories"];

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
                SerializerType = section.SerializerType,
                RequestTimeout = section.RequestTimeout
            };

            if (section.Queues.Count < 1)
            {
                throw new ConfigurationErrorsException(Resources.QueuesEmpty);
            }

            foreach (QueueElement queueElement in section.Queues)
            {
                result.AddQueue(new QueueConfiguration(queueElement.Name, queueElement.Uri));
            }

            if (section.QueueFactories != null)
            {
                foreach (QueueFactoryElement queueFactoryElement in section.QueueFactories)
                {
                    var type = Type.GetType(queueFactoryElement.Type);

                    Guard.Against<ConfigurationErrorsException>(type == null,
                        string.Format(Esb.Resources.UnknownTypeException, queueFactoryElement.Type));

                    result.AddQueueFactoryType(type);
                }

                result.ScanForQueueFactories = section.QueueFactories.Scan;
            }

            return result;
        }
    }
}