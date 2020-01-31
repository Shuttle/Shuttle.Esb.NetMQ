using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.NetMQ
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