using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQServerConfiguration : INetMQServerConfiguration
    {
        private readonly Dictionary<string, QueueConfiguration> _queues = new Dictionary<string, QueueConfiguration>();
        private readonly List<Type> _queueFactoryTypes = new List<Type>();

        public void AddQueue(QueueConfiguration queueConfiguration)
        {
            Guard.AgainstNull(queueConfiguration, nameof(queueConfiguration));

            _queues.Add(queueConfiguration.Name, queueConfiguration);
        }

        public int Port { get; set; }
        public string SerializerType { get; set;  }

        public IEnumerable<QueueConfiguration> Queues => _queues.Values.Select(item => item).ToList();
        public IEnumerable<Type> QueueFactoryTypes => _queueFactoryTypes.AsReadOnly();
        public bool ScanForQueueFactories { get; set; }

        public QueueConfiguration GetQueue(string name)
        {
            Guard.AgainstNull(name, nameof(name));

            if (!_queues.TryGetValue(name, out var result))
            {
                throw new ApplicationException(string.Format(NetMQ.Resources.UnknownQueueNameException, name));
            }

            return result;
        }

        public void AddQueueFactoryType(Type type)
        {
            Guard.AgainstNull(type, nameof(type));

            _queueFactoryTypes.Add(type);
        }
    }
}