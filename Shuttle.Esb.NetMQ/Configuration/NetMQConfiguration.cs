using System;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQConfiguration : INetMQConfiguration
    {
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}