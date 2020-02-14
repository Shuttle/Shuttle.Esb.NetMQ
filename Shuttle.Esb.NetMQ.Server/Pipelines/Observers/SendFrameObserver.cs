using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Esb.NetMQ.Server.Processing;

namespace Shuttle.Esb.NetMQ.Server.Pipelines.Observers
{
    public interface ISendFrameObserver : IPipelineObserver<OnSendFrame>
    {
    }

    public class SendFrameObserver : ISendFrameObserver
    {
        private readonly INetMQRequestServer _netMQRequestServer;

        public SendFrameObserver(INetMQRequestServer netMQRequestServer)
        {
            Guard.AgainstNull(netMQRequestServer, nameof(netMQRequestServer));

            _netMQRequestServer = netMQRequestServer;
        }

        public void Execute(OnSendFrame pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var transportMessageStream = state.GetTransportMessageStream();

            Guard.AgainstNull(transportMessageStream, nameof(transportMessageStream));

            _netMQRequestServer.SendFrameStream(transportMessageStream);

            transportMessageStream.Dispose();
        }
    }
}