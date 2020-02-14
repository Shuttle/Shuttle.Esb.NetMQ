using System.IO;

namespace Shuttle.Esb.NetMQ.Server.Processing
{
    public interface INetMQRequestServer
    {
        Stream GetFrameStream();
        void SendFrameStream(Stream stream);
    }
}