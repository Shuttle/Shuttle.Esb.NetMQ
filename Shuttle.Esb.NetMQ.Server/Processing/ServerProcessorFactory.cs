using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.NetMQ.Server.Processing
{
    public class ServerProcessorFactory : IProcessorFactory
    {
        private readonly IPipelineFactory _pipelineFactory;

        public ServerProcessorFactory(IPipelineFactory pipelineFactory)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));

            _pipelineFactory = pipelineFactory;
        }

        public IProcessor Create()
        {
            return new ServerProcessor(_pipelineFactory);
        }
    }
}