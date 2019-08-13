using System;
using System.Configuration;
using NetMQ;
using Shuttle.Core.Configuration;

namespace Shuttle.Esb.NetMQ.Server.Configuration
{
    public class NetMQSection : ConfigurationSection
    {
        [ConfigurationProperty("queues", IsRequired = true)]
        public QueueElementCollection Queues => (QueueElementCollection)this["queues"];

        public static INetMQConfiguration GetConfiguration()
        {
            var section = ConfigurationSectionProvider.Open<NetMQSection>("shuttle", "netmq");
            var result = new NetMQConfiguration();

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