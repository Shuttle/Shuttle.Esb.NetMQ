using System.Collections.Generic;

namespace Shuttle.Esb.NetMQ.Server.Configuration
{
    public interface INetMQConfiguration
    {
        IEnumerable<QueueConfiguration> Queues { get; }
        QueueConfiguration FindQueue(string name);
    }
}