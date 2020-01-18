using System.IO;

namespace Shuttle.Esb.NetMQ.Server
{
    public interface INetMQRequestServer
    {
        Stream GetFrameStream();
    }
}