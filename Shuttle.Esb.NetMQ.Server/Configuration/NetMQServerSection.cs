using System;
using System.Configuration;
using Shuttle.Core.Configuration;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQServerSection : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int) this["port"];

        [ConfigurationProperty("serializerType", IsRequired = false, DefaultValue = "")]
        public string SerializerType => (string) this["serializerType"];

        [ConfigurationProperty("queues", IsRequired = true)]
        public QueueElementCollection Queues => (QueueElementCollection) this["queues"];

        [ConfigurationProperty("queueFactories", IsRequired = false, DefaultValue = null)]
        public QueueFactoriesElement QueueFactories => (QueueFactoriesElement) this["queueFactories"];

        public static INetMQServerConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<NetMQServerSection>("shuttle", "netmq");

            if (section == null)
            {
                throw new ConfigurationErrorsException(Resources.ConfigurationSectionMissing);
            }

            var result = new NetMQServerConfiguration
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