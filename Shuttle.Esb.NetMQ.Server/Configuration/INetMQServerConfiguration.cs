using System;
using System.Collections.Generic;

namespace Shuttle.Esb.NetMQ.Server
{
    public interface INetMQServerConfiguration
    {
        int Port { get; }
        string SerializerType { get; }
        IEnumerable<QueueConfiguration> Queues { get; }
        QueueConfiguration GetQueue(string name);
        IEnumerable<Type> QueueFactoryTypes { get; }
        bool ScanForQueueFactories { get; }
    }
}