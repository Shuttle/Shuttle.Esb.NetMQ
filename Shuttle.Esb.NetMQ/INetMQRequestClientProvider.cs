using System.Net;

namespace Shuttle.Esb.NetMQ
{
    public interface INetMQRequestClientProvider
    {
        INetMQRequestClient Get(IPEndPoint ipEndPoint);
    }
}