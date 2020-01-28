namespace Shuttle.Esb.NetMQ
{
    public class GetMessageResponse : Response
    {
        public byte[] StreamBytes { get; set; }
        public object AcknowledgementToken { get; set; }
    }
}