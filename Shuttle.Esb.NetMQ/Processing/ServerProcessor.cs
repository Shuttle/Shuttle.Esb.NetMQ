using System;
using System.Threading;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.NetMQ
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
                pipeline.State.Replace(StateKeys.CancellationToken, cancellationToken);

                pipeline.Execute();
            }
            finally
            {
                _pipelineFactory.ReleasePipeline(pipeline);
            }
        }
    }
}