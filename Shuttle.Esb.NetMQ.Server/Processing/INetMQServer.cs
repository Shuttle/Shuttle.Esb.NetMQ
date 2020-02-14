namespace Shuttle.Esb.NetMQ.Server.Processing
{
    public interface INetMQServer
    {
        INetMQServer Start();
        void Stop();
    }
}