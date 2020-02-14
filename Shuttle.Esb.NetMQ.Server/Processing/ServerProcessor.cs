using System.Threading;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;
using Shuttle.Esb.NetMQ.Server.Pipelines;

namespace Shuttle.Esb.NetMQ.Server.Processing
{
    public class ServerProcessor : IProcessor
    {
        private readonly IPipelineFactory _pipelineFactory;

        public ServerProcessor(IPipelineFactory pipelineFactory)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));

            _pipelineFactory = pipelineFactory;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            var pipeline = _pipelineFactory.GetPipeline<ServerPipeline>();

            try
            {
                pipeline.State.ResetWorking();
                pipeline.State.Replace(Pipelines.StateKeys.CancellationToken, cancellationToken);

                pipeline.Execute();
            }
            finally
            {
                _pipelineFactory.ReleasePipeline(pipeline);
            }
        }
    }
}