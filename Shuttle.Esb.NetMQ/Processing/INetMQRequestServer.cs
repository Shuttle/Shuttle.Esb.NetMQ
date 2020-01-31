using System.IO;

namespace Shuttle.Esb.NetMQ
{
    public interface INetMQRequestServer
    {
        Stream GetFrameStream();
        void SendFrameStream(Stream stream);
    }
}