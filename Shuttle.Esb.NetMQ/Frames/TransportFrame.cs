using System;

namespace Shuttle.Esb.NetMQ
{
    public class TransportFrame
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string QueueName { get; set; }
        public string MessageType { get; set; }
        public byte[] Message { get; set; }
        public string AssemblyQualifiedName { get; set; }
    }
}