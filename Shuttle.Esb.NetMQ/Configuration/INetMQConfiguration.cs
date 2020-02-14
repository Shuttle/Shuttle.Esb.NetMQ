using System;

namespace Shuttle.Esb.NetMQ
{
    public interface INetMQConfiguration
    {
        TimeSpan RequestTimeout { get; }
    }
}