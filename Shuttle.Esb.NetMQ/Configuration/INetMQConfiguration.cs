using System.Collections.Generic;

namespace Shuttle.Esb.NetMQ.Server
{
    public interface INetMQConfiguration
    {
        int Port { get; }
        IEnumerable<QueueConfiguration> Queues { get; }
        QueueConfiguration GetQueue(string name);
    }
}