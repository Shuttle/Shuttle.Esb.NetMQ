using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ
{
    public interface IGetFrameObserver : IPipelineObserver<OnGetFrame>
    {
    }

    public class GetFrameObserver : IGetFrameObserver
    {
        private readonly INetMQRequestServer _netMQRequestServer;

        public GetFrameObserver(INetMQRequestServer netMQRequestServer)
        {
            Guard.AgainstNull(netMQRequestServer, nameof(netMQRequestServer));

            _netMQRequestServer = netMQRequestServer;
        }

        public void Execute(OnGetFrame pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var stream = _netMQRequestServer.GetFrameStream();

            if (stream == null)
            {
                pipelineEvent.Pipeline.Abort();
            }
            else
            {
                state.SetWorking();
                state.Replace(StateKeys.Stream, stream);
            }
        }
    }
}