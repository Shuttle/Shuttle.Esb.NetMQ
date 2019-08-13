using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server.Configuration
{
    public class NetMQConfiguration : INetMQConfiguration
    {
        private readonly Dictionary<string, QueueConfiguration> _queues = new Dictionary<string, QueueConfiguration>();

        public void AddQueue(QueueConfiguration queueConfiguration)
        {
            Guard.AgainstNull(queueConfiguration, nameof(queueConfiguration));

            _queues.Add(queueConfiguration.Name, queueConfiguration);
        }

        public IEnumerable<QueueConfiguration> Queues => _queues.Values.Select(item => item).ToList();

        public QueueConfiguration FindQueue(string name)
        {
            Guard.AgainstNull(name, nameof(name));

            _queues.TryGetValue(name, out var result);

            return result;
        }
    }
}