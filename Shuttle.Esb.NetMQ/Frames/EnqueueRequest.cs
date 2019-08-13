namespace Shuttle.Esb.NetMQ.Frames
{
    public class EnqueueRequest
    {
        public TransportMessage TransportMessage { get; set; }
        public byte[] StreamBytes { get; set; }
    }
}