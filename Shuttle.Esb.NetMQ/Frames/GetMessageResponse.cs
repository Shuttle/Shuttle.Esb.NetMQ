namespace Shuttle.Esb.NetMQ.Frames
{
    public class GetMessageResponse : Response
    {
        public byte[] StreamBytes { get; set; }
        public object AcknowledgementToken { get; set; }
    }
}