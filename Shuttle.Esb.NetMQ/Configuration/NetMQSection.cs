using System;
using System.Configuration;
using Shuttle.Core.Configuration;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQSection : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int)this["port"];

        [ConfigurationProperty("queues", IsRequired = true)]
        public QueueElementCollection Queues => (QueueElementCollection)this["queues"];

        public static INetMQConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<NetMQSection>("shuttle", "netmq");
            var result = new NetMQConfiguration
            {
                Port = section.Port
            };

            foreach (QueueElement queueElement in section.Queues)
            {
                Uri uri;

                try
                {
                    uri = new Uri(queueElement.Uri);
                }
                catch (Exception ex)
                {
                    throw new NetMQException(string.Format(Resources.InvalidQueueUriExcepttion, queueElement.Name,
                        queueElement.Uri));
                }

                result.AddQueue(new QueueConfiguration(queueElement.Name, uri));
            }

            return result;
        }
    }
}