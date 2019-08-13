namespace Shuttle.Esb.NetMQ.Frames
{
    public class TransportFrame
    {
        public string QueueName { get; set; }
        public string TypeName { get; set; }
        public byte[] Data { get; set; }
    }
}