using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQConfiguration : INetMQConfiguration
    {
        private readonly Dictionary<string, QueueConfiguration> _queues = new Dictionary<string, QueueConfiguration>();

        public void AddQueue(QueueConfiguration queueConfiguration)
        {
            Guard.AgainstNull(queueConfiguration, nameof(queueConfiguration));

            _queues.Add(queueConfiguration.Name, queueConfiguration);
        }

        public int Port { get; set; }

        public IEnumerable<QueueConfiguration> Queues => _queues.Values.Select(item => item).ToList();

        public QueueConfiguration GetQueue(string name)
        {
            Guard.AgainstNull(name, nameof(name));

            if (!_queues.TryGetValue(name, out var result))
            {
                throw new ApplicationException(string.Format(Resources.UnknownQueueNameException, name));
            }

            return result;
        }
    }
}