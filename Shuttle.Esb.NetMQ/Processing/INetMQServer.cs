namespace Shuttle.Esb.NetMQ
{
    public interface INetMQServer
    {
        INetMQServer Start();
        void Stop();
    }
}