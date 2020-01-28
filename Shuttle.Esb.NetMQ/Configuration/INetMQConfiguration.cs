using System.Collections.Generic;

namespace Shuttle.Esb.NetMQ
{
    public interface INetMQConfiguration
    {
        int Port { get; }
        string SerializerType { get; }
        IEnumerable<QueueConfiguration> Queues { get; }
        QueueConfiguration GetQueue(string name);
    }
}