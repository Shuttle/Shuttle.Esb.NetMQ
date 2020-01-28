namespace Shuttle.Esb.NetMQ
{
    public class EnqueueRequest
    {
        public TransportMessage TransportMessage { get; set; }
        public byte[] StreamBytes { get; set; }
    }
}