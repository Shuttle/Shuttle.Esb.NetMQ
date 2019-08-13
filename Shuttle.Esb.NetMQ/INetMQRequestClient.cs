using Shuttle.Esb.NetMQ.Frames;

namespace Shuttle.Esb.NetMQ
{
    public interface INetMQRequestClient
    {
        TResponse GetResponse<TResponse>(object request, string queueName);
    }
}