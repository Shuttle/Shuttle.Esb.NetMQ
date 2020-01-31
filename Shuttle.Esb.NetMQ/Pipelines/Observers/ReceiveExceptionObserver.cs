using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ
{
    public interface IReceiveExceptionObserver : IPipelineObserver<OnPipelineException>
    {
    }

    public class ReceiveExceptionObserver : IReceiveExceptionObserver
    {
        private readonly INetMQRequestServer _netMqRequestServer;
        private readonly ISerializer _serializer;
        private readonly ILog _log;

        public ReceiveExceptionObserver(INetMQRequestServer netMQRequestServer, ISerializer serializer)
        {
            Guard.AgainstNull(netMQRequestServer, nameof(netMQRequestServer));
            Guard.AgainstNull(serializer, nameof(serializer));

            _netMqRequestServer = netMQRequestServer;
            _serializer = serializer;

            _log = Log.For(this);
        }

        public void Execute(OnPipelineException pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            _log.Error(pipelineEvent.Pipeline.Exception.AllMessages());

            using (var stream = _serializer.Serialize(new Response
            {
                Exception = pipelineEvent.Pipeline.Exception.AllMessages()
            }))
            {
                _netMqRequestServer.SendFrameStream(stream);
            }

            pipelineEvent.Pipeline.MarkExceptionHandled();
        }
    }
}